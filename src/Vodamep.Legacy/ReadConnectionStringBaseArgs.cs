using PowerArgs;

namespace Vodamep.Legacy
{
    public abstract class ReadConnectionStringBaseArgs : ReadBaseArgs
    {
        [DefaultValue(@".")]
        public string Server { get; set; }

        [ArgRequired]
        public string Database { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public string GetSqlServerCS() => string.IsNullOrEmpty(this.User) ?
            $"Server={this.Server};Database={this.Database};Trusted_Connection=True;" :
            $"Server={this.Server};Database={this.Database};User Id={this.User};Password={Password}";
    }
}
