using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace CodeSharp.MongoToLINQ
{
    class ExampleProgram
    {
        static void Main(string[] args)
        {
            var users = new List<User>()
            {
                new User() {Id = 1, Age = 20, Status = 'o', UserId = 1, Asset = new Asset() { Id = 20, Rules = Enumerable.Range(10,10).Select(i => new Rule() { Id = i}).ToArray()}},
                new User() {Id = 2, Age = 50, Status = 'o', UserId = 2, Asset = new Asset() { Id = 1, Rules = Enumerable.Range(5,10).Select(i => new Rule() { Id = i}).ToArray()}},
                new User() {Id = 55, Age = 10, Status = 'A', UserId = 2, Asset = new Asset() { Id = 3}},
                new User() {Id = 2, Age = 110, Status = 'A', UserId = 2},
                new User() {Id = 2, Age = 70, Status = 'A', UserId = 2, Asset = new Asset() { Id = 100}}
            }.AsQueryable();

            var json = Queries.Size;
            var token = JToken.Parse(json);

            IQueryParser<User> parser = new QueryParser<User>();
            var whereExpression = parser.ParseWhere(token);

            foreach (var user in users.Where(whereExpression))
            {
                Console.WriteLine(user);
            }

            Console.WriteLine(json);
        }
    }


    public class User
    {

        public int Id { get; set; }

        public int UserId { get; set; }

        public int Age { get; set; }

        public char Status { get; set; }

        public Asset Asset { get; set; }

        public override string ToString()
        {
            return string.Format("[ Id : {0}, UserId : {1}, Age : {2}, Status {3} ]", Id, UserId, Age, Status);
        }
    }

    public class Asset
    {

        public int Id { get; set; }

        public Rule[] Rules { get; set; }

    }

    public class Rule
    {
        public int Id { get; set; }
    }
}
