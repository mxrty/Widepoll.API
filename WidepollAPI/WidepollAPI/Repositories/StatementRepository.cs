namespace WidepollAPI
{
    public interface IStatementRepository
    {
        public IReadOnlyCollection<Statement> GetAll();
        //TODO: null or tryget?
        public Statement? GetById(Guid id); 
        public void Save(Statement statement);
    }

    public class StatementRepository : IStatementRepository
    {
        public Dictionary<Guid, Statement> Statements { get; set; } 

        public StatementRepository()
        {
            Statements = new Dictionary<Guid, Statement>();
        }

        public IReadOnlyCollection<Statement> GetAll()
        {
            return Statements.Values.ToList();
        }

        public void Save(Statement statement)
        {
            Statements.Add(statement.Id, statement);
        }

        public Statement? GetById(Guid id)
        {
            Statements.TryGetValue(id, out Statement statement);
            return statement;
        }
    }
}
