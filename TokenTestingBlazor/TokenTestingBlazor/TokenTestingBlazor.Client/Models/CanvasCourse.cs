namespace TokenTestingBlazor.Client.Models
{
    public class CanvasCourse
    {
        public int id { get; set; }
        public string name { get; set; }
        public string uuid { get; set; }
        public string workflow_state { get; set; }
        public int total_students { get; set; }
        public List<CourseEnrollment> enrollments { get; set; }
    }

    public class CourseEnrollment
    {
        public string type { get; set; }
        public string role { get; set; }
        public int role_id { get; set; }
        public string enrollment_state { get; set; }
    }
}
