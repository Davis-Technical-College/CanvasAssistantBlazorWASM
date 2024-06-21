namespace TokenTestingBlazor.Client
{
    public class CourseSelector
    {
        private int? SelectedCourse = null;

        private string? SelectedCourseName = null;

        public void SetSelection(int id, string name)
        {
            SelectedCourse = id;
            SelectedCourseName = name;
        }

        public (int?, string?) GetSelection()
        {
            return (SelectedCourse, SelectedCourseName);
        }
    }
}
