using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App3.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page12 : ContentPage
    {
        public Page12()
        {
            
            InitializeComponent();
            BuildReportHtml();

            

        }


        public string ReportHtml { get; set; }

        private void BuildReportHtml()
        {

            

            var chartConfigScript = GetChartScript();
            var html = GetHtmlWithChartConfig(chartConfigScript);
            ReportHtml = html;

            L1.Text = html;

    
            WebView1.Source = html;
          


        }

        private string GetHtmlWithChartConfig(string chartConfig)
        {
            var inlineStyle = "style=\"width:100%;height:100%;\"";
            var chartJsScript = "<script src=\"https://cdnjs.cloudflare.com/ajax/libs/Chart.js/2.7.1/Chart.bundle.min.js\"></script>";
            var chartConfigJsScript = $"<script>{chartConfig}</script>";
            var chartContent = $@"<div id=""chart-container"" {inlineStyle}>
  <canvas id=""chart"" />
</div>";
            var document = $@"<html style=""width:97%;height:100%;"">
  <head>{chartJsScript}</head>
  <body {inlineStyle}>
    {chartContent}
    {chartConfigJsScript}
  </body>
</html>";
            return document;
        }


        private string GetChartScript()
        {
            var chartConfig = GetSpendingChartConfig();
            var script = $@"var config = {chartConfig};
window.onload = function() {{
  var canvasContext = document.getElementById(""chart"").getContext(""2d"");
  new Chart(canvasContext, config);
}};";
            return script;
        }











        private string GetSpendingChartConfig()
        {
            var config = new
            {
                type = "pie",
                data = GetPieChartData(),
                options = new
                {
                    responsive = true,
                    maintainAspectRatio = false,
                    legend = new
                    {
                        position = "top"
                    },
                    animation = new
                    {
                        animateScale = true
                    }
                }
            };
            var jsonConfig = JsonConvert.SerializeObject(config);
            return jsonConfig;
        }



        private object GetPieChartData()
        {
            var colors = GetDefaultColors();
            var labels = new[] { "Groceries", "Car", "Flat", "Electronics", "Entertainment", "Insurance" };
            var randomGen = new Random();
            var dataPoints = Enumerable.Range(0, labels.Length)
                .Select(i => randomGen.Next(5, 25))
                .ToList();
            var data = new
            {
                datasets = new[]
                {
                    new
                    {
                        label = "Spending",
                        data = dataPoints,
                        backgroundColor = dataPoints.Select((d, i) =>
                        {
                            var color = colors[i % colors.Count];
                            return $"rgb({color.Item1},{color.Item2},{color.Item3})";
                        })
                    }
                },
                labels
            };
            return data;
        }

        private List<Tuple<int, int, int>> GetDefaultColors()
        {
            return new List<Tuple<int, int, int>>
            {
                new Tuple<int, int, int>(255, 99, 132),
                new Tuple<int, int, int>(255, 159, 64),
                new Tuple<int, int, int>(255, 205, 86),
                new Tuple<int, int, int>(75, 192, 192),
                new Tuple<int, int, int>(54, 162, 235),
                new Tuple<int, int, int>(153, 102, 255),
                new Tuple<int, int, int>(201, 203, 207)
            };
        }

    }
}