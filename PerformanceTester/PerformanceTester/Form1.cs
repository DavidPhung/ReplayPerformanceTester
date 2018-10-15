using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LiveCharts;
using LiveCharts.Wpf;
using MathNet.Numerics.Distributions;

namespace PerformanceTester
{
    public partial class Form1 : Form
    {
        private GUIDataMonitor monitor;

        public Form1(GUIDataMonitor monitor)
        {
            InitializeComponent();
            this.monitor = monitor;

            respondTimeChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Respond Time",
                    Values = new ChartValues<double>()
                },

                new LineSeries
                {
                    Title = "Average",
                    Values = new ChartValues<double>(),
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromArgb(0,1,1,1)),
                }
            };

            averageMemoryChart.Series = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = "Average Memory",
                    Values = new ChartValues<double>()
                },

                new LineSeries
                {
                    Title = "Average",
                    Values = new ChartValues<double>(),
                    PointGeometry = null,
                    LineSmoothness = 0,
                    Fill = new System.Windows.Media.SolidColorBrush(
                        System.Windows.Media.Color.FromArgb(0,1,1,1)),
                }
            };

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            respondTimeBgWorker.RunWorkerAsync();
            averageMemoryBgWorker.RunWorkerAsync();
        }

        private void respondTimeBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            int index = 0;
            List<double> values = new List<double>();
            while (true)
            {
                List<long> l = monitor.GetLatestRunTimeMillis(ref index);
                foreach (var v in l)
                {
                    respondTimeChart.Series[0].Values.Add(v / 1000d);
                    values.Add(v / 1000d);
                }
                responseTime_nValueLabel.Invoke((MethodInvoker)delegate {
                    responseTime_nValueLabel.Text = values.Count.ToString(); });
                if (values.Count > 1)
                {
                    double avg = values.Average();
                    double standardError = StandardError(values);
                    double confidenceInterval95 = ConfidenceInterval95(values, standardError);
                    double lowerBound = avg - confidenceInterval95;
                    double upperBound = avg + confidenceInterval95;
                    //update average line
                    respondTimeChart.Series[1].Values.Clear();
                    for (int i = 0; i < values.Count; i++)
                    {
                        respondTimeChart.Series[1].Values.Add(Math.Round(avg, 3));
                    }

                    //update texts
                    responseTime_averageValueLabel.Invoke((MethodInvoker)delegate {
                        responseTime_averageValueLabel.Text = avg.ToString("0.000");
                    });

                    responseTime_standardErrorValueLabel.Invoke((MethodInvoker)delegate {
                        responseTime_standardErrorValueLabel.Text = standardError.ToString("0.000");
                    });

                    responseTime_confidenceIntervalValueLabel.Invoke((MethodInvoker)delegate {
                        responseTime_confidenceIntervalValueLabel.Text =
                        avg.ToString("0.000") + " \u00B1" + confidenceInterval95.ToString("0.000") + "\r\n" + 
                        lowerBound.ToString("0.000") + " to " + upperBound.ToString("0.000");
                    });
                }
            }
        }


        private void averageMemoryBgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            int index = 0;
            List<double> values = new List<double>();
            while (true)
            {
                List<double> l = monitor.GetLatestAvgMems(ref index);
                foreach (var v in l)
                {
                    averageMemoryChart.Series[0].Values.Add(v);
                    values.Add(v);
                }
                averageMemory_nValueLabel.Invoke((MethodInvoker)delegate {
                    averageMemory_nValueLabel.Text = values.Count.ToString();
                });
                if (values.Count > 1)
                {
                    double avg = values.Average();
                    double standardError = StandardError(values);
                    double confidenceInterval95 = ConfidenceInterval95(values, standardError);
                    double lowerBound = avg - confidenceInterval95;
                    double upperBound = avg + confidenceInterval95;

                    //update average line
                    averageMemoryChart.Series[1].Values.Clear();
                    for (int i = 0; i < values.Count; i++)
                    {
                        averageMemoryChart.Series[1].Values.Add(Math.Round(avg, 3));
                    }

                    //update texts
                    averageMemory_averageValueLabel.Invoke((MethodInvoker)delegate {
                        averageMemory_averageValueLabel.Text = avg.ToString("N0");
                    });

                    averageMemory_standardErrorValueLabel.Invoke((MethodInvoker)delegate {
                        averageMemory_standardErrorValueLabel.Text = standardError.ToString("N0");
                    });

                    averageMemory_confidenceIntervalValueLabel.Invoke((MethodInvoker)delegate {
                        averageMemory_confidenceIntervalValueLabel.Text =
                        avg.ToString("N0") + " \u00B1" + confidenceInterval95.ToString("N0") + "\r\n" +
                        lowerBound.ToString("N0") + " to " + upperBound.ToString("N0");
                    });
                }
            }
        }

        private double StandardDeviation(List<double> l)
        {
            double avg = l.Average();
            double sum = 0;
            foreach (var d in l)
            {
                sum += (d - avg) * (d - avg);
            }
            double standardDeviation = Math.Sqrt(sum / (l.Count - 1));
            return standardDeviation;
        }

        private double StandardError(List<double> l)
        {
            return StandardDeviation(l) / Math.Sqrt(l.Count);
        }

        private double ConfidenceInterval95(List<double> l, double standardError)
        {
            return -StudentT.InvCDF(0, 1, l.Count - 1, 0.025) * standardError;
        }
    }
}
