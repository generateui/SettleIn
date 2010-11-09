using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace SettleInCommon.Board
{
    [DataContract]
    public enum EBoardCreatedType
    {
        [EnumMember(Value = "Template")]
        Template,
        [EnumMember(Value = "CustomCreated")]
        CustomCreated,
        [EnumMember(Value = "Downloaded")]
        Downloaded,
        [EnumMember(Value = "Official")]
        Official
    };
}
