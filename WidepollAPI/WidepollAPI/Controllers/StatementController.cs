using Microsoft.AspNetCore.Mvc;
using WidepollAPI.Ports;

namespace WidepollAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StatementController : ControllerBase
    {
        private readonly IStatementRepository _repository;
        private readonly ILogger<StatementController> _logger;

        public StatementController(IStatementRepository repository, ILogger<StatementController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("GetStatements")]
        public IEnumerable<Statement> GetStatements()
        {
            return _repository.GetAll();
        }

        [HttpGet("GetStatement")]
        public ActionResult<Statement> GetStatement(Guid id)
        {
            var statement = _repository.GetById(id);
            if (statement is null) return NotFound();
            return statement;
        }

        [HttpPost("Create")]
        public Guid CreateStatement([FromBody] StatementDto dto)
        {
            var statement = new Statement();
            statement.Restore(dto);
            _repository.Save(statement);
            return statement.Id;
        }
    }
}