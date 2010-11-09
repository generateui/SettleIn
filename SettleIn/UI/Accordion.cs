using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SettleIn.UI
{
    public class Accordion : StackPanel
    {
        static Accordion()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Accordion),
                new FrameworkPropertyMetadata(typeof(Accordion)));
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.InitializeAccordion();
        }

        protected void InitializeAccordion()
        {
            Expander selectedExpander;
            foreach (UIElement element in this.Children)
            {
                selectedExpander = element as Expander;
                if (selectedExpander != null)
                {
                    selectedExpander.Expanded += new RoutedEventHandler(selectedExpander_Expanded);
                }
            }
        }

        void selectedExpander_Expanded(object sender, RoutedEventArgs e)
        {
            Expander selectedExpander = sender as Expander;
            Expander otherExpander = null;
            ContentPresenter contentPresenter = null;
            double totalExpanderHeight = 0;

            if (selectedExpander != null)
            {
                foreach (UIElement element in this.Children)
                {
                    otherExpander = element as Expander;
                    if (otherExpander != null & otherExpander != selectedExpander)
                    {
                        if (otherExpander.IsExpanded)
                        {
                            contentPresenter = otherExpander.Template.FindName("ExpandSite", otherExpander) as ContentPresenter;
                            if (contentPresenter != null)
                                totalExpanderHeight -= contentPresenter.ActualHeight;
                        }
                        otherExpander.IsExpanded = false;
                        totalExpanderHeight += otherExpander.ActualHeight;
                    }
                }

                if (selectedExpander.IsExpanded)
                {
                    contentPresenter = selectedExpander.Template.FindName("ExpandSite", selectedExpander) as ContentPresenter;
                    if (contentPresenter != null)
                        contentPresenter.Height = this.ActualHeight - totalExpanderHeight - selectedExpander.ActualHeight;
                }
            }
        }
    }
}
