using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class GetStudentByIdDto
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime CreateTime { get; set; }
        public string CreateTimeToString { get { return CreateTime.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public int ClassNo { get; set; }
        public string ClassName { get; set; }
    }
}
