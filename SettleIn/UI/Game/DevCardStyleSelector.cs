using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using SettleInCommon.Gaming.DevCards;

namespace SettleIn.UI.Game
{
    public class DevCardStyleSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            DevelopmentCard devcard = item as DevelopmentCard;
            if (devcard is Soldier)
                return (DataTemplate)Core.Instance.DataTemplates["soldierTemplate"];
            if (devcard is Monopoly)
                return (DataTemplate)Core.Instance.DataTemplates["monopolyTemplate"];
            if (devcard is YearOfPlenty)
                return (DataTemplate)Core.Instance.DataTemplates["yearOfPlentyTemplate"];
            if (devcard is RoadBuilding)
                return (DataTemplate)Core.Instance.DataTemplates["roadBuildingTemplate"];
            if (devcard is VictoryPoint)
                return (DataTemplate)Core.Instance.DataTemplates["victoryPointTemplate"];
            throw new Exception("Whoa");

        } 
    }
}
