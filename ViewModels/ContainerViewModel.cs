using ImperialMetricConversion.CommonUtils;
using ImperialMetricConversion.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Media3D;

namespace ImperialMetricConversion.ViewModels
{
    public class ContainerViewModel : INotifier
    {
        #region Constructor

        public ContainerViewModel()
        {
            MetricToImperialPanelVis = Visibility.Collapsed;
            ImperialToMetricPanelVis = Visibility.Visible;
        }

        #endregion

        #region Variables

        public bool IsSuccess = false;
        public AsynchronousCall asyncCallToSearch = null;

        #endregion

        #region Properties

        #region Imperial To Metric Properties

        #region String Properties

        private string imperial_Input;
        public string Imperial_Input
        {
            get { return imperial_Input; }
            set
            {
                imperial_Input = value;
                NotifyProperty("Imperial_Input");
            }
        }

        private string metric_Output;
        public string Metric_Output
        {
            get { return metric_Output; }
            set
            {
                metric_Output = value;
                NotifyProperty("Metric_Output");
            }
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                NotifyProperty("ErrorMessage");
            }
        }

        #endregion

        #region Boolean Properties

        private bool isEnableConversionWindow = true;
        public bool IsEnableConversionWindow
        {
            get { return isEnableConversionWindow; }
            set
            {
                isEnableConversionWindow = value;
                NotifyProperty("IsEnableConversionWindow");
            }
        }

        private bool isCheckedImperialToMetric = true;
        public bool IsCheckedImperialToMetric
        {
            get { return isCheckedImperialToMetric; }
            set
            {
                isCheckedImperialToMetric = value;
                if (value)
                {
                    MetricToImperialPanelVis = Visibility.Collapsed;
                    ImperialToMetricPanelVis = Visibility.Visible;
                }
                NotifyProperty("IsCheckedImperialToMetric");
            }
        }

        private bool isCheckedMetricToImperial;
        public bool IsCheckedMetricToImperial
        {
            get { return isCheckedMetricToImperial; }
            set
            {
                isCheckedMetricToImperial = value;
                if (value)
                {
                    if(Metric_Input == 0)
                    {
                        ErrorMessage = "";
                        ErrorMessageVis = Visibility.Collapsed;
                    }
                    ImperialToMetricPanelVis = Visibility.Collapsed;
                    MetricToImperialPanelVis = Visibility.Visible;
                }
                NotifyProperty("IsCheckedMetricToImperial");
            }
        }

        #endregion

        #region Visibility Properties

        private Visibility metricValueVis = Visibility.Collapsed;
        public Visibility MetricValueVis
        {
            get { return metricValueVis; }
            set
            {
                metricValueVis = value;
                NotifyProperty("MetricValueVis");
            }
        }

        private Visibility progressBarVis = Visibility.Collapsed;
        public Visibility ProgressBarVis
        {
            get { return progressBarVis; }
            set
            {
                progressBarVis = value;
                NotifyProperty("ProgressBarVis");
            }
        }

        private Visibility errorMessageVis = Visibility.Collapsed;
        public Visibility ErrorMessageVis
        {
            get { return errorMessageVis; }
            set
            {
                errorMessageVis = value;
                NotifyProperty("ErrorMessageVis");
            }
        }

        private Visibility metricToImperialPanelVis;
        public Visibility MetricToImperialPanelVis
        {
            get { return metricToImperialPanelVis; }
            set
            {
                metricToImperialPanelVis = value;
                NotifyProperty("MetricToImperialPanelVis");
            }
        }

        #endregion

        #endregion

        #region Metric To Imperial Properties

        #region Decimal Properties

        private decimal metric_Input;
        public decimal Metric_Input
        {
            get { return metric_Input; }
            set
            {
                metric_Input = value;
                NotifyProperty("Metric_Input");
            }
        }

        #endregion

        #region Integer Properties

        private int imperial_Foot;
        public int Imperial_Foot
        {
            get { return imperial_Foot; }
            set
            {
                imperial_Foot = value;
                NotifyProperty("Imperial_Foot");
            }
        }

        private int imperial_Inch;
        public int Imperial_Inch
        {
            get { return imperial_Inch; }
            set
            {
                imperial_Inch = value;
                NotifyProperty("Imperial_Inch");
            }
        }

        private int imperial_One16;
        public int Imperial_One16
        {
            get { return imperial_One16; }
            set
            {
                imperial_One16 = value;
                NotifyProperty("Imperial_One16");
            }
        }

        #endregion

        #region Visibility Properties

        private Visibility imperialValueVis = Visibility.Collapsed;
        public Visibility ImperialValueVis
        {
            get { return imperialValueVis; }
            set
            {
                imperialValueVis = value;
                NotifyProperty("ImperialValueVis");
            }
        }

        private Visibility imperialToMetricPanelVis = Visibility.Visible;
        public Visibility ImperialToMetricPanelVis
        {
            get { return imperialToMetricPanelVis; }
            set
            {
                imperialToMetricPanelVis = value;
                NotifyProperty("ImperialToMetricPanelVis");
            }
        }

        #endregion

        #endregion

        #endregion

        #region Commands

        public RelayCommand SearchCommand { get { return new RelayCommand(AsyncCallToSearch, true); } }
        public RelayCommand ClearCommand { get { return new RelayCommand(ClearSearch, true); } }
        

        #endregion

        #region Methods

        public void AsyncCallToSearch()
        {
            if (asyncCallToSearch != null) return;
            ErrorMessageVis = Visibility.Collapsed;
            IsSuccess = false;
            if (IsCheckedImperialToMetric)
            {
                MetricValueVis = Visibility.Collapsed;

                if(!ValidateImperialValues(Imperial_Input))
                    return;
            }
            else
            {
                ImperialValueVis = Visibility.Collapsed;
                if (Metric_Input == 0)
                {
                    ErrorMessage = "Metric value is required.";
                    ErrorMessageVis = Visibility.Visible;
                    return;
                }
            }
            
            IsEnableConversionWindow = false;
            ProgressBarVis = Visibility.Visible;
            asyncCallToSearch = new AsynchronousCall(action: this.SearchResult,
                                               completed: this.CallBackToSearch,
                                           error: (ex) => Debug.WriteLine(ex.StackTrace));
            asyncCallToSearch.Execute(this);
        }

        /// <summary>
        /// Search result
        /// </summary>
        private void SearchResult()
        {
            try
            {
                if (IsCheckedImperialToMetric)
                {
                    Metric_Output = string.IsNullOrEmpty(Imperial_Input) ? "" : CalculateMetricValues(Imperial_Input);

                    if (!string.IsNullOrEmpty(Metric_Output) && Convert.ToDecimal(Metric_Output) > 0)
                    {
                        IsSuccess = true;
                    }
                }
                else
                {
                    Tuple<int, int, int> MetricValues;

                    MetricValues = CalculateImperialValues(Metric_Input);

                    if (MetricValues != null && MetricValues.Item1 > 0)
                    {
                        Imperial_Foot = MetricValues.Item1;
                        Imperial_Inch = MetricValues.Item2;
                        Imperial_One16 = MetricValues.Item3;
                        IsSuccess = true;
                    }
                }
            }
            catch (Exception)
            {
                IsSuccess = false;
                ErrorMessage = "Error while getting Metric values. Try again later.";
                ErrorMessageVis = Visibility.Visible;
                MetricValueVis = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Call back to search
        /// </summary>
        public void CallBackToSearch()
        {
            asyncCallToSearch = null;
            IsEnableConversionWindow = true;
            ProgressBarVis = Visibility.Collapsed;
            if (IsSuccess)
            {
                if (IsCheckedImperialToMetric)
                {
                    MetricValueVis = Visibility.Visible;
                }
                else
                {
                    ImperialValueVis = Visibility.Visible;
                }
                ErrorMessageVis = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Clear Search Result
        /// </summary>
        private void ClearSearch()
        {
            if (IsCheckedImperialToMetric)
            {
                Imperial_Input = "";
                Metric_Output = "";
            }
            else
            {
                Metric_Input = 0;
                Imperial_Foot = 0;
                Imperial_Inch = 0;
                Imperial_One16 = 0;
            }
        }

        /// <summary>
        /// Validate Imperial Values
        /// </summary>
        /// <param name="Height"></param>
        /// <param name="Width"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        private bool ValidateImperialValues(string imperial_Value)
        {
            bool isValid = true;
            try
            {
                if (string.IsNullOrEmpty(imperial_Value))
                {
                    ErrorMessage = "Imperial value is required.";
                    ErrorMessageVis = Visibility.Visible;
                    return isValid = false;
                }

                if(!imperial_Value.Replace(" ", "").All(char.IsDigit))
                {
                    ErrorMessage = "Imperial value should be numeric only.";
                    ErrorMessageVis = Visibility.Visible;
                    return isValid = false;
                }

                if(imperial_Value.Trim() == "0")
                {
                    ErrorMessage = "Imperial value should be greater than 0.";
                    ErrorMessageVis = Visibility.Visible;
                    return isValid = false;
                }

                if (!string.IsNullOrEmpty(imperial_Value) && imperial_Value.Length > 0)
                {
                    if (imperial_Value.Length >= 3 && imperial_Value.Length <= 7)
                    {
                        if (string.IsNullOrEmpty(imperial_Value.Substring(0, 3).Trim()))
                        {
                            ErrorMessage = "Foot is required in Imperial value.";
                            ErrorMessageVis = Visibility.Visible;
                            return isValid = false;
                        }
                    }
                        
                    //if (imperial_Value.Length <= 3 && !imperial_Value.Contains(" "))
                    //if (imperial_Value.Length <= 3)
                    //{
                    //    return isValid = true;
                    //}

                    string[] splitValue = imperial_Value.Split(' ');

                    if (imperial_Value.Split(' ').Length > 0 && imperial_Value.Split(' ').Length <= 3)
                    {
                        return isValid = true;
                    }

                    if (imperial_Value.Length < 7 || imperial_Value.Length > 7)
                    {
                        ErrorMessage = "Imperial value should be 7 Character length.";
                        ErrorMessageVis = Visibility.Visible;
                        return isValid = false;
                    }
                    
                }
            }
            catch (Exception)
            {
                return isValid = false;
            }
            return isValid;
        }

        /// <summary>
        /// Calculate Metric Values
        /// </summary>
        /// <param name="Input_Range"></param>
        /// <returns></returns>
        private string CalculateMetricValues(string imperial_Value)
        {
            decimal Feet_Range = 0, Inch_Range = 0, One16_Range = 0;
            string MetricValue = "";

            int foot = 0, inch = 0, one16 = 0;

            try
            {
                if (imperial_Value.Length == 7)
                {
                    foot =  !string.IsNullOrEmpty(imperial_Value.Substring(0, 3).Trim()) ? Convert.ToInt32(imperial_Value.Substring(0, 3)) : 0;
                    inch =  !string.IsNullOrEmpty(imperial_Value.Substring(3, 2).Trim()) ? Convert.ToInt32(imperial_Value.Substring(3, 2)) : 0;
                    one16 = !string.IsNullOrEmpty(imperial_Value.Substring(5, 2).Trim()) ? Convert.ToInt32(imperial_Value.Substring(5, 2)) : 0;
                }
                else
                {
                    string[] splitValue = imperial_Value.Trim().Split(' ');

                    if (splitValue.Length > 0)
                    {
                        foot = !string.IsNullOrEmpty(splitValue.ElementAtOrDefault(0)) ? Convert.ToInt32(splitValue[0]) : 0;
                        inch = !string.IsNullOrEmpty(splitValue.ElementAtOrDefault(1)) ? Convert.ToInt32(splitValue[1]) : 0;
                        one16 = !string.IsNullOrEmpty(splitValue.ElementAtOrDefault(2)) ? Convert.ToInt32(splitValue[2]) : 0;
                    }
                }

                //if (imperial_Value.Length == 7)
                //{
                //    foot = string.IsNullOrEmpty(imperial_Value.Substring(0, 3).Trim()) ? 0 : Convert.ToInt32(imperial_Value.Substring(0, 3));
                //    inch = string.IsNullOrEmpty(imperial_Value.Substring(3, 2).Trim()) ? 0 : Convert.ToInt32(imperial_Value.Substring(3, 2));
                //    one16 = string.IsNullOrEmpty(imperial_Value.Substring(5, 2).Trim()) ? 0 : Convert.ToInt32(imperial_Value.Substring(5, 2));
                //}
                ////else if (imperial_Value.Length <= 3 && !imperial_Value.Contains(" "))
                //else if (imperial_Value.Length <= 3)
                //{
                //    string[] splitValue = imperial_Value.Trim().Split(' ');

                //    if(splitValue.Length == 1)
                //    {
                //        foot = Convert.ToInt32(imperial_Value.Trim());
                //    }
                //    else if (splitValue.Length > 1 && splitValue.Length <= 3)
                //    {
                //        foot = splitValue.ElementAtOrDefault(0) == null ? 0 : Convert.ToInt32(splitValue[0]);
                //        inch = splitValue.ElementAtOrDefault(1) == null ? 0 : Convert.ToInt32(splitValue[1]);
                //        one16 = splitValue.ElementAtOrDefault(2) == null ? 0 : Convert.ToInt32(splitValue[2]);
                //    }
                //}

                if (foot > 0)
                {
                    Feet_Range = foot * 3048;
                    Feet_Range = Feet_Range / 10;

                    Inch_Range = inch * 254;
                    Inch_Range = Inch_Range / 10;

                    One16_Range = one16 * 15875;
                    One16_Range = One16_Range / 10000;
                }
                if (Feet_Range > 0)
                {
                    MetricValue = Convert.ToString(Feet_Range + Inch_Range + One16_Range);
                }
            }
            catch (Exception)
            {
                return MetricValue = "";
            }
            return MetricValue;
        }

        /// <summary>
        /// Calculate Imperial Values
        /// </summary>
        /// <param name="Input_Range"></param>
        /// <returns></returns>
        private Tuple<int, int, int> CalculateImperialValues(decimal metric_Value)
        {
            decimal Feet_192 = 0, Dec10_204 = 0, Inch_207 = 0, Dec10_208 = 0, X16_218 = 0, Dec10_222 = 0,
                    Inch_223 = 0, X16_224 = 0, Inch_greaterThan_16 = 0, Inch_EqualTo_0 = 0;
            try
            {
                Feet_192 = Math.Floor((metric_Value * 10) / 3048);

                Dec10_204 = Math.Floor(metric_Value - ((Feet_192 * 3048) / 10));

                Inch_207 = Math.Floor((Dec10_204 * 10) / 254);

                Dec10_208 = Dec10_204 - ((Inch_207 * 254) / 10);

                X16_218 = Math.Ceiling(((@Dec10_208 * 10000) / 15875));


                Dec10_222 = Math.Floor(@X16_218 / 16);


                Inch_223 = X16_218 > 15 ? Inch_207 + Dec10_222 : Inch_207;
                X16_224 = X16_218 > 15 ? Math.Floor(X16_218 - (Dec10_222 * 16)) : X16_218;

                Inch_greaterThan_16 = X16_224 == 16 ? Inch_greaterThan_16 = Inch_223 + 1 : Inch_223;
                Inch_EqualTo_0 = X16_224 == 16 ? 0 : X16_224;
            }
            catch (Exception)
            {
                return Tuple.Create(0, 0, 0);
            }
            return Tuple.Create(Convert.ToInt32(Feet_192), Convert.ToInt32(Inch_223), Convert.ToInt32(X16_224));
        }

        #endregion
    }
}
