using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public abstract class Triangle
        {
            public int Side1 { get; set; }
            public int Side2 { get; set; }
            public int Degree { get; set; }
            public Triangle(int side1, int side2, int degree)
            {
                Side1 = side1;
                Side2 = side2;
                Degree = degree;
            }
            public abstract double Area(); // площадь
            public abstract double Perimeter(); // периметр
            virtual public double Side3()
            {
                return Math.Round(Math.Sqrt((Side1 * Side1) + (Side2 * Side2) - (2 * Side1 * Side2 * Math.Round(Math.Cos(Degree * (Math.PI) / 180)))));
            }
        }


        class Isosceles : Triangle
        {
            public Isosceles(int side1, int side2, int degree)
                : base(side1, side2, degree) { }
            public override double Area()
            {
                return Side1 * Side2 * Math.Sin(Degree * (Math.PI) / 180) / 2;
            }
            public override double Perimeter()
            {
                return Side1 + Side2 + Side3();
            }
            public override string ToString()
            {
                return $"   Сторона 1: {Side1} Сторона 2: {Side2} Угол между ними: {Degree}"; ;
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                Isosceles c = obj as Isosceles;
                if (c as Isosceles == null)
                    return false;
                return c.Side1 == this.Side1;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        class Equilateral : Triangle
        {
            public Equilateral(int side1, int side2, int degree)
                : base(side1, side2, degree) { }

            public override double Area()
            {
                double p = Perimeter() / 2;

                return Math.Sqrt(p * (p - Side1) * (p - Side2) * (p - Side2));
            }

            public override double Perimeter()
            {
                return Side1 + Side2 + Side2;
            }
            public override string ToString()
            {
                return $"   Сторона 1: {Side1} Сторона 2: {Side2} Угол между ними: {Degree}"; ;
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                Equilateral c = obj as Equilateral;
                if (c as Equilateral == null)
                    return false;
                return c.Side1 == this.Side1;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        class Straight : Triangle
        {
            public Straight(int side1, int side2, int degree)
                : base(side1, side2, degree) { }

            public override double Area()
            {
                double p = Perimeter() / 2;
                return Math.Sqrt(p * (p - Side1) * (p - Side2) * (p - Side3()));
            }

            public override double Perimeter()
            {
                return Side1 + Side2 + Side3();
            }
            public override string ToString()
            {
                return $"   Сторона 1: {Side1} Сторона 2: {Side2} Угол между ними: {Degree}"; ;
            }
            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;
                Straight c = obj as Straight;
                if (c as Straight == null)
                    return false;
                return c.Degree == this.Degree;
            }
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }

        List<Triangle> triangles = new List<Triangle>();
        int i = 0;
        public MainWindow()
        {
            InitializeComponent();

            triangles.Add(new Isosceles(4, 4, 110));
            triangles.Add(new Isosceles(8, 8, 35));
            triangles.Add(new Isosceles(15, 15, 80));
            triangles.Add(new Equilateral(5, 5, 60));
            triangles.Add(new Equilateral(42, 42, 60));
            triangles.Add(new Equilateral(7, 7, 60));
            triangles.Add(new Straight(3, 4, 90));
            triangles.Add(new Straight(5, 7, 90));
            triangles.Add(new Straight(10, 17, 90));

        }  

    private void Main_Bt_Click(object sender, RoutedEventArgs e) //поиск периметра и площади
        {
            if ((Int32.TryParse(Index_Tb.Text, out int number)) == false)
            {
                MessageBox.Show("Значение должно быть задано цифрой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Index_Tb.Text = "";
            }
            else if ((triangles.Count() < int.Parse(Index_Tb.Text) - 1) || (int.Parse(Index_Tb.Text) < 0))
            {
                MessageBox.Show("Введён некорректный индекс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Index_Tb.Text = "";
            }
            else
            {
                try
                {
                    var item = triangles[int.Parse(Index_Tb.Text)];
                    Main_LB.Items.Add($"Периметр и площадь элемента под индексом {Index_Tb.Text} равны " +
                        $"{item.Perimeter()} и {item.Area()} соответственно");
                    Index_Tb.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Обратитесь к разработчику: " + ex.Message, "Неизвестная " +
                        "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void MEquals_Bt_Click(object sender, RoutedEventArgs e) //выбор поиска периметра и площади
        {
            Main_LB.Items.Clear();
            foreach (var cur in triangles)
            {
                Main_LB.Items.Add($"{i}" + cur.ToString());
                i++;
            }
            i = 0;
            Side1_TB.Visibility = Visibility.Hidden;
            Side2_TB.Visibility = Visibility.Hidden;
            Degree_TB.Visibility = Visibility.Hidden;
            Add_TB.Visibility = Visibility.Hidden;
            Index_Lb.Visibility = Visibility.Visible;
            Index_Tb.Visibility = Visibility.Visible;
            Delete_Bt.Visibility = Visibility.Hidden;
            Main_Bt.Visibility = Visibility.Visible;
            Add_Bt.Visibility = Visibility.Hidden;
        }

        private void MDelete_Bt_Click(object sender, RoutedEventArgs e) //выбор удаления объекта
        {
            Main_LB.Items.Clear();
            foreach (var cur in triangles)
            {
                Main_LB.Items.Add($"{i}" + cur.ToString());
                i++;
            }
            i = 0;
            Side1_TB.Visibility = Visibility.Hidden;
            Side2_TB.Visibility = Visibility.Hidden;
            Degree_TB.Visibility = Visibility.Hidden;
            Add_TB.Visibility = Visibility.Hidden;
            Index_Lb.Visibility = Visibility.Visible;
            Index_Tb.Visibility = Visibility.Visible;
            Delete_Bt.Visibility = Visibility.Visible;
            Main_Bt.Visibility = Visibility.Hidden;
            Add_Bt.Visibility = Visibility.Hidden;
        }

        private void MAdd_Bt_Click(object sender, RoutedEventArgs e) //выбор добавления объекта
        {
            Main_LB.Items.Clear();
            foreach (var cur in triangles)
            {
                Main_LB.Items.Add($"{i}" + cur.ToString());
                i++;
            }
            i = 0;
            Side1_TB.Visibility = Visibility.Visible;
            Side2_TB.Visibility = Visibility.Visible;
            Degree_TB.Visibility = Visibility.Visible;
            Add_TB.Visibility = Visibility.Visible;
            Index_Lb.Visibility = Visibility.Hidden;
            Index_Tb.Visibility = Visibility.Hidden;
            Delete_Bt.Visibility = Visibility.Hidden;
            Main_Bt.Visibility = Visibility.Hidden;
            Add_Bt.Visibility = Visibility.Visible;
        }

        private void Delete_Bt_Click(object sender, RoutedEventArgs e) //удаление объекта
        {
            if ((Int32.TryParse(Index_Tb.Text, out int number)) == false) 
            {
                MessageBox.Show("Значение должно быть задано цифрой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Index_Tb.Text = "";
            }
            else if (triangles.Count() == 0)
            {
                MessageBox.Show("В списке нет значений", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Index_Tb.Text = "";
            }
            else if ((triangles.Count() < int.Parse(Index_Tb.Text) - 1) || (int.Parse(Index_Tb.Text) < 0))
            {
                MessageBox.Show("Введён некорректный индекс", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Index_Tb.Text = "";
            }
            else
            {
                try
                {
                    var item = triangles[int.Parse(Index_Tb.Text)];
                    triangles.Remove(item);
                    Main_LB.Items.Clear();
                    foreach (var cur in triangles)
                    {
                        Main_LB.Items.Add($"{i}" + cur.ToString());
                        i++;
                    }
                    i = 0;
                    Index_Tb.Text = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Обратитесь к разработчику: " + ex.Message, "Неизвестная " +
                        "ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Add_Bt_Click(object sender, RoutedEventArgs e) //добавление объекта
        {
            if (Is_RB.IsChecked == true) //равнобедренного
            {

                if ((Int32.TryParse(Side1_TB.Text, out int number)) == false || 
                    (Int32.TryParse(Side2_TB.Text, out number)) == false || (Int32.TryParse(Degree_TB.Text, out number)) == false)
                {
                    MessageBox.Show("Значение должно быть задано цифрой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Side1_TB.Text = "";
                    Side2_TB.Text = "";
                    Degree_TB.Text = "";
                }
                else
                {
                    Isosceles n = new Isosceles(int.Parse(Side1_TB.Text), int.Parse(Side2_TB.Text), int.Parse(Degree_TB.Text));
                    if ((n.Side1 == n.Side2) || (n.Side2 == n.Side3()) || (n.Side1 == n.Side3()))
                    {
                        triangles.Add(n);
                        Main_LB.Items.Clear();
                        foreach (var cur in triangles)
                        {
                            Main_LB.Items.Add($"{i}" + cur.ToString());
                            i++;
                        }
                        i = 0;
                    }
                    else
                    {
                        MessageBox.Show($"Вы ввели значения не соответствующее равнобедренному " +
                            "треугольнику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); ; ;
                        Side1_TB.Text = "";
                        Side2_TB.Text = "";
                        Degree_TB.Text = "";
                    }
                }
            }
            if (Eq_RB.IsChecked == true) //равностороннего
            {

                if ((Int32.TryParse(Side1_TB.Text, out int number)) == false ||
                    (Int32.TryParse(Side2_TB.Text, out number)) == false || (Int32.TryParse(Degree_TB.Text, out number)) == false)
                {
                    MessageBox.Show("Значение должно быть задано цифрой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Side1_TB.Text = "";
                    Side2_TB.Text = "";
                    Degree_TB.Text = "";
                }
                else
                {
                    Isosceles n = new Isosceles(int.Parse(Side1_TB.Text), int.Parse(Side2_TB.Text), int.Parse(Degree_TB.Text));
                    if ((n.Side1 == n.Side2) && (n.Degree == 60))
                    {
                        triangles.Add(n);
                        Main_LB.Items.Clear();
                        Main_LB.Items.Clear();
                        foreach (var cur in triangles)
                        {
                            Main_LB.Items.Add($"{i}" + cur.ToString());
                            i++;
                        }
                        i = 0;
                    }
                    else
                    {
                        MessageBox.Show($"Вы ввели значения не соответствующее равностороннему " +
                            "треугольнику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); ; ;
                        Side1_TB.Text = "";
                        Side2_TB.Text = "";
                        Degree_TB.Text = "";
                    }
                }
            }
            if (St_RB.IsChecked == true) //прямоугольного
            {

                if ((Int32.TryParse(Side1_TB.Text, out int number)) == false ||
                     (Int32.TryParse(Side2_TB.Text, out number)) == false || (Int32.TryParse(Degree_TB.Text, out number)) == false)
                {
                    MessageBox.Show("Значение должно быть задано цифрой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    Side1_TB.Text = "";
                    Side2_TB.Text = "";
                    Degree_TB.Text = "";
                }
                else
                {
                    Isosceles n = new Isosceles(int.Parse(Side1_TB.Text), int.Parse(Side2_TB.Text), int.Parse(Degree_TB.Text));
                    double k = (n.Side1 * n.Side1) + (n.Side2 * n.Side2);
                    if (Math.Round(k) == n.Side3() * n.Side3())
                    {
                        triangles.Add(n);
                        Main_LB.Items.Clear();
                        foreach (var cur in triangles)
                        {
                            Main_LB.Items.Add($"{i}" + cur.ToString());
                            i++;
                        }
                        i = 0;
                    }
                    else
                    {
                        MessageBox.Show($"Вы ввели значения не соответствующее прямоугольному " +
                            $"треугольнику", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); ; ;
                        Side1_TB.Text = "";
                        Side2_TB.Text = "";
                        Degree_TB.Text = "";
                    }
                }
            }
        }
    }
}
