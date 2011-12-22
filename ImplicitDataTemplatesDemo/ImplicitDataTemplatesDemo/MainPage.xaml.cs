using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

namespace ImplicitDataTemplatesDemo
{
	public partial class MainPage : UserControl
	{
		public MainPage()
		{
            this.DataContext = this;
			// Required to initialize variables
			InitializeComponent();


            People = new List<IPerson>{
                new MichaelSync(),
                new ElenaSync(),
                new ShwesinSync(),
            }; 
		}

        public IList<IPerson> People { get; set; }
	}

    public interface IContact {
        string Name { get; set; }
        BitmapImage Logo { get; set; }
    }


    public interface IPerson {
         string Name { get;  }
         string Description { get;  }
         BitmapImage ProfilePhoto { get; }
    }

    public class MichaelSync : IPerson {
        public string Name {
            get {
                return "michael sync";
            }
        }

        public string Description {
            get {
                return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in mauris ut ligula pharetra tempus a iaculis odio. Curabitur mi lacus, iaculis vitae pharetra at, gravida eget odio. Morbi sit amet molestie eros. Fusce imperdiet vehicula ipsum. Nam ut tellus.";   
            }            
        }

        public BitmapImage ProfilePhoto {
            get {
                return new BitmapImage(new Uri("MichaelSync.jpg", UriKind.Relative));
            }
        }
    }

    public class ShwesinSync : IPerson {
        public string Name {
            get {
                return "shwesin sync";
            }
        }

        public string Description {
            get {
                return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in mauris ut ligula pharetra tempus a iaculis odio. Curabitur mi lacus, iaculis vitae pharetra at, gravida eget odio. Morbi sit amet molestie eros. Fusce imperdiet vehicula ipsum. Nam ut tellus.";
            }
        }
        public BitmapImage ProfilePhoto {
            get {
                return new BitmapImage(new Uri("ShwesinSync.JPG", UriKind.Relative));
            }
        }
    }

    public class ElenaSync : IPerson {
        public string Name {
            get {
                return "elena sync";
            }
        }

        public string Description {
            get {
                return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus in mauris ut ligula pharetra tempus a iaculis odio. Curabitur mi lacus, iaculis vitae pharetra at, gravida eget odio. Morbi sit amet molestie eros. Fusce imperdiet vehicula ipsum. Nam ut tellus.";
            }
        }
        public BitmapImage ProfilePhoto {
            get {
                return new BitmapImage(new Uri("ElenaSync.JPG", UriKind.Relative));
            }
        }
    }
}