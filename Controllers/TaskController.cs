using Microsoft.AspNetCore.Mvc;
using WebApplication4.ApplicationDBContext;
using WebApplication4.Entities;

namespace WebApplication4.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController : Controller
    {

        private readonly LocalDbContext _localdb;
        private readonly ServerDbContext _serverdb;
        public TaskController(LocalDbContext localdb ,ServerDbContext serverdb)
        {
            _localdb = localdb;
            _serverdb = serverdb;
        }

        [HttpGet]
        [Route("getalltasks")]

        public IActionResult GetTask()
        {
            var getalltasks = _localdb.Tasks.Where(x => x.IsDeleted == 0).ToList();
            return Ok(getalltasks);
        }

        [HttpGet]
        [Route("getsyncqueue")]

        public IActionResult GetSyncQueue()
        {
            var getsynctask = _localdb.SyncQ.ToList();
            return Ok(getsynctask);
        }
        [HttpPost]
        [Route("tasks")]

        public IActionResult CreateTask(TaskRequest request)
        {
            var createnewtask = new TaskEntity()
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Completed = 0,
                IsDeleted = 0,
                SyncStatus = "pending",
                CreatedAt = DateTime.UtcNow,



            };

            var createsynctask = new SyncQueue()
            {
                Id = Guid.NewGuid(),
                TaskId = createnewtask.Id,
                Title = request.Title,
                Operation = "Created",
                Description = request.Description,
                CreatedAt = createnewtask.CreatedAt,
                SyncStatus = "pending",
                
            };

            _localdb.Tasks.Add(createnewtask);
            _localdb.SyncQ.Add(createsynctask);
            _localdb.SaveChanges();
            return Ok(createnewtask);
        }

        [HttpPut]
        [Route("tasks:{id}")]

        public IActionResult UpdateTask(Guid id, UpdateTask request)
        {
            var gettask = _localdb.Tasks.FirstOrDefault(x => x.Id == id);
            if (gettask != null)
            {
                if (!String.IsNullOrEmpty(request.Title))
                {
                    gettask.Title = request.Title;

                }
                if (!String.IsNullOrEmpty(request.Description))
                {
                    gettask.Description = request.Description;
                }
                if (request.Completed != null)
                {
                    gettask.Completed = (int)request.Completed;
                }

                gettask.UpdatedAt = DateTime.UtcNow;
                var updatesynctask = new SyncQueue()
                {
                    Id = Guid.NewGuid(),
                    TaskId = gettask.Id,
                    Title = gettask.Title,
                    Operation = "Update",
                    Description = gettask.Description,
                    CreatedAt = DateTime.UtcNow,
                    SyncStatus = "pending",
                    
                };
                _localdb.Tasks.Update(gettask);
                _localdb.SyncQ.Add(updatesynctask);
                _localdb.SaveChanges();
                return Ok(gettask);
            }
            else
            {
                return NotFound("Task not found");

            }
        }
        [HttpPost]
        [Route("sync/batch")]
        public IActionResult PostSync()
        {
            var gettaskssync = _localdb.SyncQ.Where(k => k.SyncStatus == "pending")
                .OrderBy(x => x.CreatedAt)
                .ToList();
            var syncresponse = new Syncresponse();
            List<ErrorMessage> errors = new List<ErrorMessage>();

            foreach (var item in gettaskssync)
            {
                var gettaskfromlocal = _localdb.Tasks.FirstOrDefault(x => x.Id == item.TaskId);
                if (gettaskfromlocal == null)
                {
                    return Ok("error");
                }
                if (item.Operation == "Created")
                {
                    try
                    {

                        var createdtask = new TaskTable()
                        {
                            TaskId = item.TaskId.ToString(),
                            Title = item.Title,
                            Description = item.Description,
                            Completed = gettaskfromlocal.Completed,
                            IsDeleted = gettaskfromlocal.IsDeleted,
                            CreatedAt = gettaskfromlocal.CreatedAt,
                            ServerId = $"srv{Guid.NewGuid()}"
                        };
                        _serverdb.TaskTable.Add(createdtask);
                        _serverdb.SaveChanges();
                        item.SyncStatus = "Synced";
                        gettaskfromlocal.ServerId = createdtask.ServerId;
                        gettaskfromlocal.LastSyncedAt = DateTime.UtcNow;
                        gettaskfromlocal.SyncStatus = "Synced";
                        _localdb.Tasks.Update(gettaskfromlocal);
                        _localdb.SyncQ.Update(item);
                        _localdb.SaveChanges();
                        syncresponse.synced_items += 1;
                    }
                    catch (Exception ex)
                    {
                        syncresponse.failed_items += 1;

                    }
                }
                else if (item.Operation == "Update")
                {
                    try
                    {
                        var gettaskfromserver = _serverdb.TaskTable.FirstOrDefault(x => x.TaskId == item.TaskId.ToString());
                        if (gettaskfromserver != null)
                        {

                            gettaskfromserver.Title = item.Title;
                            gettaskfromserver.Description = item.Description;
                            gettaskfromserver.Completed = gettaskfromlocal.Completed;
                            gettaskfromserver.IsDeleted = gettaskfromlocal.IsDeleted;


                            if (gettaskfromserver?.UpdatedAt != null && gettaskfromserver.UpdatedAt < gettaskfromlocal.UpdatedAt)
                            {
                                gettaskfromserver.UpdatedAt = (DateTime)gettaskfromlocal.UpdatedAt;
                                gettaskfromlocal.LastSyncedAt = DateTime.UtcNow;
                                gettaskfromlocal.SyncStatus = "Synced";
                                item.SyncStatus = "Synced";
                                _localdb.SyncQ.Update(item);
                                _localdb.Tasks.Update(gettaskfromlocal);
                                _localdb.SaveChanges();
                                _serverdb.TaskTable.Update(gettaskfromserver);
                                _serverdb.SaveChanges();
                                throw new Exception("Conflict detected");
                            }
                            gettaskfromserver.UpdatedAt = (DateTime)gettaskfromlocal.UpdatedAt;
                            gettaskfromlocal.LastSyncedAt = DateTime.UtcNow;
                            gettaskfromlocal.SyncStatus = "Synced";

                            item.SyncStatus = "Synced";
                            _localdb.SyncQ.Update(item);
                            _localdb.Tasks.Update(gettaskfromlocal);
                            _localdb.SaveChanges();
                            _serverdb.TaskTable.Update(gettaskfromserver);
                            _serverdb.SaveChanges();
                            item.SyncStatus = "Synced";
                            syncresponse.synced_items += 1;
                        }
                    }
                    catch (Exception ex)
                    {
                        syncresponse.failed_items += 1;
                        var errormessage = new ErrorMessage()
                        {
                            task_id = item.TaskId.ToString(),
                            operation = item.Operation,
                            error = "Conflict resolved using last write-wins",
                            timestamp = DateTime.UtcNow

                        };
                        errors.Add(errormessage);
                        

                    }
                }


            }
            syncresponse.errors = errors;
            return Ok(syncresponse);
        }

        [HttpGet]
        [Route("status")]

        public IActionResult CheckStatus()
        {
            var gettotalsyncsize = _localdb.SyncQ.Count();
            var getpending = _localdb.SyncQ.Where(x => x.SyncStatus == "pending").Count();
            var getlastsynceditem = _localdb.Tasks.OrderByDescending(x => x.LastSyncedAt).FirstOrDefault();

            var getstatus = new Status()
            {
                pending_sync_count = getpending,
                last_sync_timestamp = getlastsynceditem?.LastSyncedAt,
                is_online = 1,
                sync_queu_size = gettotalsyncsize
            };
            return Ok(getstatus);
        }
    }
}
