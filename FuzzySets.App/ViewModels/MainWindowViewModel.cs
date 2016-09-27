using FolderSerialization.Client.Models;
using FuzzySets.App.Models;
using FuzzySets.Logic.Models;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static System.Math;

namespace FuzzySets.App.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private double x0 = -20;
        private double x1 = 50;
        private double dx = 0.1;
        private PlotModel _plotModel;
        private FuzzySet _a;
        private FuzzySet _b;
        private bool _drawAB;
        private Func<double, double> _mfA => x => MembershipFunctionA(x);
        private Func<double, double> _mfB => x => MembershipFunctionB(x);

        public MainWindowViewModel()
        {
            PlotModel = new PlotModel();
            PlotModel.Axes.Clear();
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                AbsoluteMaximum = 1.5,
                AbsoluteMinimum = -0.5,
                IsZoomEnabled = false,
                MinimumRange = 2,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid
            });

            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom,
                IsZoomEnabled = false,
                AbsoluteMaximum = x1,
                AbsoluteMinimum = x0,
                MinimumRange = x1 - x0,
                MinorGridlineStyle = LineStyle.Dot,
                MajorGridlineStyle = LineStyle.Solid
            });

            A = new FuzzySet(_mfA);
            B = new FuzzySet(_mfB);
        }

        public PlotModel PlotModel
        {
            get { return _plotModel; }
            set { SetProperty(ref _plotModel, value); }
        }

        public FuzzySet A
        {
            get { return _a; }
            set { SetProperty(ref _a, value); }
        }
        public FuzzySet B
        {
            get { return _b; }
            set { SetProperty(ref _b, value); }
        }

        public ObservableCollection<CommandItem> CommandItems =>
            new ObservableCollection<CommandItem>()
            {
                new CommandItem() { Text="Fuzzy set A", Command = fuzzySetA },
                new CommandItem() { Text="Fuzzy set B", Command = fuzzySetB },
                new CommandItem() { Text="Standard complement", Command = standardComplement },
                new CommandItem() { Text="Concentration", Command = concentration },
                new CommandItem() { Text="Difference", Command = difference },
                new CommandItem() { Text="Intersection of first type", Command = intersectionOfFirstType },
                new CommandItem() { Text="Intersection of second type", Command = intersectionOfSecondType },
                new CommandItem() { Text="Intersection of third type", Command = intersectionOfThirdType },
                new CommandItem() { Text="Union of first type", Command = unionOfFirstType },
                new CommandItem() { Text="Union of second type", Command = unionOfSecondType },
                new CommandItem() { Text="Union of third type", Command = unionOfThirdType },

                new CommandItem() { Text="Morgano I1 U1", Command = morganoI1U1 },
                new CommandItem() { Text="Morgano I1 U2", Command = morganoI1U2 },
                new CommandItem() { Text="Morgano I1 U3", Command = morganoI1U3 },

                new CommandItem() { Text="Morgano I2 U1", Command = morganoI2U1 },
                new CommandItem() { Text="Morgano I2 U2", Command = morganoI2U2 },
                new CommandItem() { Text="Morgano I2 U3", Command = morganoI2U3 },

                new CommandItem() { Text="Morgano I3 U1", Command = morganoI3U1 },
                new CommandItem() { Text="Morgano I3 U2", Command = morganoI3U2 },
                new CommandItem() { Text="Morgano I3 U3", Command = morganoI3U3 },
            };

        private ICommand fuzzySetA => new DelegateCommand(obj => FuzzySetA());
        private ICommand fuzzySetB => new DelegateCommand(obj => FuzzySetB());
        private ICommand standardComplement => new DelegateCommand(obj => StandardComplement());
        private ICommand concentration => new DelegateCommand(obj => Concentration());
        private ICommand difference => new DelegateCommand(obj => Difference());
        private ICommand intersectionOfFirstType => new DelegateCommand(obj => IntersectionOfFirstType());
        private ICommand intersectionOfSecondType => new DelegateCommand(obj => IntersectionOfSecondType());
        private ICommand intersectionOfThirdType => new DelegateCommand(obj => IntersectionOfThirdType());
        private ICommand unionOfFirstType => new DelegateCommand(obj => UnionOfFirstType());
        private ICommand unionOfSecondType => new DelegateCommand(obj => UnionOfSecondType());
        private ICommand unionOfThirdType => new DelegateCommand(obj => UnionOfThirdType());

        private ICommand morganoI1U1 => new DelegateCommand(obj => MorganoI1U1());
        private ICommand morganoI1U2 => new DelegateCommand(obj => MorganoI1U2());
        private ICommand morganoI1U3 => new DelegateCommand(obj => MorganoI1U3());

        private ICommand morganoI2U1 => new DelegateCommand(obj => MorganoI2U1());
        private ICommand morganoI2U2 => new DelegateCommand(obj => MorganoI2U2());
        private ICommand morganoI2U3 => new DelegateCommand(obj => MorganoI2U3());

        private ICommand morganoI3U1 => new DelegateCommand(obj => MorganoI3U1());
        private ICommand morganoI3U2 => new DelegateCommand(obj => MorganoI3U2());
        private ICommand morganoI3U3 => new DelegateCommand(obj => MorganoI3U3());


        public bool DrawAB
        {
            get { return _drawAB; }
            set { SetProperty(ref _drawAB, value); }
        }

        private double MembershipFunctionB(double x)
        {
            return x <= 0 ? Exp(x) : x > 0 && x < 1 ? 1 : 1 / x;
        }

        private double MembershipFunctionA(double x)
        {
            return (Cos(x) + 1) / 2;
        }

        private void FuzzySetA()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.InvalidatePlot(true);
        }

        private void FuzzySetB()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.InvalidatePlot(true);
        }

        private void StandardComplement()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries((!B).Mf, x0, x1, dx, "Standard complement of A"));
            PlotModel.InvalidatePlot(true);
        }

        private void Concentration()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries((~B).Mf, x0, x1, dx, "Concentration of A"));
            PlotModel.InvalidatePlot(true);
        }

        private void Difference()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A / B).Mf, x0, x1, dx, "Difference"));
            PlotModel.InvalidatePlot(true);
        }

        private void IntersectionOfFirstType()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A * B).Mf, x0, x1, dx, "Intersection of first type"));
            PlotModel.InvalidatePlot(true);
        }

        private void IntersectionOfSecondType()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A & B).Mf, x0, x1, dx, "Intersection of second type"));
            PlotModel.InvalidatePlot(true);
        }

        private void IntersectionOfThirdType()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A ^ B).Mf, x0, x1, dx, "Intersection of third type"));
            PlotModel.InvalidatePlot(true);
        }

        private void UnionOfFirstType()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A + B).Mf, x0, x1, dx, "Union of first type"));
            PlotModel.InvalidatePlot(true);
        }

        private void UnionOfSecondType()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A | B).Mf, x0, x1, dx, "Union of second type"));
            PlotModel.InvalidatePlot(true);
        }

        private void UnionOfThirdType()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((A % B).Mf, x0, x1, dx, "Union of third type"));
            PlotModel.InvalidatePlot(true);
        }

        private void MorganoI1U1()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A * B)).Mf, x0, x1, dx, "!(A i1 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) + !(B)).Mf, x0, x1, dx, "!A u1 !B"));
            PlotModel.InvalidatePlot(true);
        }
        private void MorganoI1U2()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A * B)).Mf, x0, x1, dx, "!(A i1 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) | !(B)).Mf, x0, x1, dx, "!A u2 !B"));
            PlotModel.InvalidatePlot(true);
        }
        private void MorganoI1U3()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A * B)).Mf, x0, x1, dx, "!(A i1 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) % !(B)).Mf, x0, x1, dx, "!A u3 !B"));
            PlotModel.InvalidatePlot(true);
        }



        private void MorganoI2U1()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A & B)).Mf, x0, x1, dx, "!(A i2 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) + !(B)).Mf, x0, x1, dx, "!A u1 !B"));
            PlotModel.InvalidatePlot(true);
        }
        private void MorganoI2U2()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A & B)).Mf, x0, x1, dx, "!(A i2 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) | !(B)).Mf, x0, x1, dx, "!A u2 !B"));
            PlotModel.InvalidatePlot(true);
        }
        private void MorganoI2U3()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A & B)).Mf, x0, x1, dx, "!(A i2 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) % !(B)).Mf, x0, x1, dx, "!A u3 !B"));
            PlotModel.InvalidatePlot(true);
        }



        private void MorganoI3U1()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A ^ B)).Mf, x0, x1, dx, "!(A i3 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) + !(B)).Mf, x0, x1, dx, "!A u1 !B"));
            PlotModel.InvalidatePlot(true);
        }
        private void MorganoI3U2()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A ^ B)).Mf, x0, x1, dx, "!(A i3 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) | !(B)).Mf, x0, x1, dx, "!A u2 !B"));
            PlotModel.InvalidatePlot(true);
        }
        private void MorganoI3U3()
        {
            PlotModel.Series.Clear();
            PlotModel.Series.Add(new FunctionSeries(A.Mf, x0, x1, dx, "Fuzzy set A"));
            PlotModel.Series.Add(new FunctionSeries(B.Mf, x0, x1, dx, "Fuzzy set B"));
            PlotModel.Series.Add(new FunctionSeries((!(A ^ B)).Mf, x0, x1, dx, "!(A i3 B)"));
            PlotModel.Series.Add(new FunctionSeries((!(A) % !(B)).Mf, x0, x1, dx, "!A u3 !B"));
            PlotModel.InvalidatePlot(true);
        }
    }
}
