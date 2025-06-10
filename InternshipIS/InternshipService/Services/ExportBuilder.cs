using InternshipService.Database.Entities;
using System.Reflection;
using System.Text;

namespace InternshipService.Services
{
    /// <summary>
    /// The class <c>ExportBuilder</c> is responsible for building the HTML report for an internship.
    /// </summary>
    public static class ExportBuilder
    {
        /// <summary>
        /// Builds the HTML report for the given internship.
        /// </summary>
        /// <param name="internship"></param>
        public static string Build(Internship internship)
        {
            var student = internship.Students.FirstOrDefault();
            var persons = internship.CompanyRelatives.Select(x => $"{x.Name} {x.LastName}").ToList();
            var teachers = internship.Teachers.Select(x => $"{x.Name} {x.LastName}").ToList();

            var htmlTemplate = LoadTemplate("ExportTemplate.html");

            htmlTemplate = htmlTemplate.Replace("{{INTERNSHIP_NAME}}", internship.Name);
            htmlTemplate = htmlTemplate.Replace("{{INTERNSHIP_ID}}", internship.Id.ToString());
            htmlTemplate = htmlTemplate.Replace("{{COMPANY}}", internship.CompanyName);
            htmlTemplate = htmlTemplate.Replace("{{CONTACTS}}",  string.Join(", ", persons ?? new()));
            htmlTemplate = htmlTemplate.Replace("{{TEACHERS}}", string.Join(", ", teachers ?? new()));
            htmlTemplate = htmlTemplate.Replace("{{STUDENT}}", student?.Name + " " + student?.LastName);
            htmlTemplate = htmlTemplate.Replace("{{DESCRIPTION}}", internship.Description);
            
            htmlTemplate = htmlTemplate
                .Replace("{{STATES_LIST}}", RenderStatesList(internship))
                .Replace("{{TASK_PAGES}}", RenderTaskPages(internship.Tasks.Where(x => x.IsReported).ToList()));

            return htmlTemplate;
        }

        /// <summary>
        /// Renders the HTML for the list of states in the internship report.
        /// </summary>
        /// <param name="states"></param>
        /// <returns></returns>
        private static string RenderStatesList(Internship internship)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<ul>");

            sb.AppendLine($"  <li>Vytvořeno: {internship.CreationDate:dd.MM.yyyy}</li>");

            if (internship.PublishedOn.HasValue)
                sb.AppendLine($"  <li>Zveřejněno: {internship.PublishedOn.Value:dd.MM.yyyy}</li>");

            if (internship.SelectedOn.HasValue)
                sb.AppendLine($"  <li>Vybráno: {internship.SelectedOn.Value:dd.MM.yyyy}</li>");

            if(internship.FinishedOn.HasValue)
                sb.AppendLine($"  <li>Dokončeno: {internship.FinishedOn.Value:dd.MM.yyyy}</li>");

            if (internship.CanceledOn.HasValue && !internship.FinishedOn.HasValue)
                sb.AppendLine($"  <li>Zrušeno: {internship.CanceledOn.Value:dd.MM.yyyy}</li>");

            sb.AppendLine("</ul>");
            return sb.ToString();
        }

        /// <summary>
        /// Renders the HTML for the task pages in the internship report.
        /// </summary>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private static string RenderTaskPages(List<InternshipTask> tasks)
        {
            var sb = new StringBuilder();
            int taskCounter = 0;

            foreach (var task in tasks)
            {
                if (taskCounter % 3 == 0)
                {
                    if (taskCounter > 0)
                        sb.AppendLine("</div>");

                    sb.AppendLine("<div class=\"page\">");
                    sb.AppendLine("<h2>Reportované úkoly</h2>");
                }

                sb.AppendLine("<div class=\"section no-break\">");
                sb.AppendLine($"  <h3>Úkol {taskCounter + 1}: {task.Name}</h3>");
                sb.AppendLine($"  <p><span class=\"label\">Termín odevzdání:</span> {task.EndsOn:dd.MM.yyyy}</p>");
                sb.AppendLine($"  <p><span class=\"label\">Splněno dne:</span> {(task.IsCompleted ? task.UpdatedDate?.ToString("dd.MM.yyyy") : "-")}</p>");
                sb.AppendLine($"  <p><span class=\"label\">Popis:</span> {task.Description}</p>");
                sb.AppendLine($"  <p><span class=\"label\">Shrnutí studenta:</span> {task.Summary}</p>");
                sb.AppendLine($"  <p><span class=\"label\">Shrnutí učitele:</span> {task.TeacherSummary}</p>");
                sb.AppendLine("</div>");

                taskCounter++;
            }

            if (taskCounter > 0)
                sb.AppendLine("</div>");

            return sb.ToString();
        }

        /// <summary>
        /// Loads the HTML template from embedded resources.
        /// </summary>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private static string LoadTemplate(string resourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fullName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(r => r.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (fullName == null)
                throw new InvalidOperationException($"Resource '{resourceName}' not found.");

            using var stream = assembly.GetManifestResourceStream(fullName);
            using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Resource stream null."));
            return reader.ReadToEnd();
        }
    }
}
