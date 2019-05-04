using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ExperenceHubApp
{
    public class Person
    {
        public string firstname;
        public string lastname;
        public float wallet;
        public List<Lesson> Lessons;
        public string login;
        public string email;
        public string password;
        public string token;
        public Guid userid;
        public string NewPassword;
        public string role;

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {
            if (Lessons == null)
            {
                Lessons = new List<Lesson>();
            }
        }
    }
}
