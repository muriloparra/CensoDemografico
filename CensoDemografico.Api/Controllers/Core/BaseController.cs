using CensoDemografico.Infra.Transactions;
using Flunt.Notifications;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CensoDemografico.Api.Controllers.Core
{
    public class BaseController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BaseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Response(object Result, IEnumerable<Notification> Notifications)
        {
            if (Notifications.Any())
            {
                return BadRequest(new
                {
                    success = false,
                    errors = Notifications.GroupBy(x => new { x.Property, x.Message })
                                .Select(x => x.FirstOrDefault())
                });
            }

            if (_unitOfWork != null)
            {
                _unitOfWork.Commit();
            }

            return Ok(new
            {
                sucess = true,
                data = Result
            });
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> TryErrors(Exception ex) =>
            BadRequest(
                new { success = false, 
                    errors = new List<Notification>(){
                        new Notification("BadRequest", "Ocorreu uma falha interna no servidor")
                    }
                }
            );
    }
}
