//---------------------------------
// RedRat_4.cs
//---------------------------------
/*
****************************************************************************
*  Copyright (c) 2023,  Skyline Communications NV  All Rights Reserved.    *
****************************************************************************

By using this script, you expressly agree with the usage terms and
conditions set out below.
This script and all related materials are protected by copyrights and
other intellectual property rights that exclusively belong
to Skyline Communications.

A user license granted for this script is strictly for personal use only.
This script may not be used in any way by anyone without the prior
written consent of Skyline Communications. Any sublicensing of this
script is forbidden.

Any modifications to this script by the user are only allowed for
personal use and within the intended purpose of the script,
and will remain the sole responsibility of the user.
Skyline Communications will not be responsible for any damages or
malfunctions whatsoever of the script resulting from a modification
or adaptation by the user.

The content of this script is confidential information.
The user hereby agrees to keep this confidential information strictly
secret and confidential and not to disclose or reveal it, in whole
or in part, directly or indirectly to any person, entity, organization
or administration without the prior written consent of
Skyline Communications.

Any inquiries can be addressed to:

	Skyline Communications NV
	Ambachtenstraat 33
	B-8870 Izegem
	Belgium
	Tel.	: +32 51 31 35 69
	Fax.	: +32 51 31 01 29
	E-mail	: info@skyline.be
	Web		: www.skyline.be
	Contact	: Ben Vandenberghe

****************************************************************************
Revision History:

DATE		VERSION		AUTHOR			COMMENTS

10/07/2023	1.0.0.1		API, Skyline	Initial version
****************************************************************************
*/

namespace RedRat_1
{
    using HelperClass;
    using IRD_Tuning_1.HelperClasses;
    using Newtonsoft.Json;
    using Skyline.DataMiner.Automation;

    /// <summary>
    /// Represents a DataMiner Automation script.
    /// </summary>
    public class Script
    {
        /// <summary>
        /// The script entry point.
        /// </summary>
        /// <param name="engine">Link with SLAutomation process.</param>
        public void Run(IEngine engine)
        {
            var channelDetails = engine.GetScriptParam(1).Value;
            var elementName = engine.GetScriptParam(2).Value;
            var epgnumber = engine.GetScriptParam(3).Value;

            var element = Helper.FindElement(engine, elementName);

            if (string.IsNullOrEmpty(epgnumber))
            {
                engine.GenerateInformation("epgnumber is null ");
                return;
            }

            Channel channel = JsonConvert.DeserializeObject<Channel>(channelDetails);

            string key = string.Format("Digibox.{0}", channel.Name);
            engine.GenerateInformation($"Epg Number To Set {epgnumber} on {key}");
            element.SetParameterByPrimaryKey(752, key, epgnumber);
            element.SetParameterByPrimaryKey(703, key, 1/*Send*/);
        }
    }
}
//---------------------------------
// Channel.cs
//---------------------------------
namespace HelperClass
{
    using System;
    using Newtonsoft.Json;

    public class Channel
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int AlarmSeverity { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid Class { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid Genre { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public int ServiceID { get; set; }

        public int SourceID { get; set; }

    }
}
//---------------------------------
// Helper.cs
//---------------------------------
namespace IRD_Tuning_1.HelperClasses
{
    using System;
    using Skyline.DataMiner.Automation;

    public class Helper
    {
        public static Element FindElement(IEngine engine, string name)
        {
            var element = engine.FindElement(name);
            if (element == null || element.IsActive == false)
            {
                throw new InvalidOperationException($"Couldn't find element {name}");
            }
            else
            {
                return element;
            }
        }

        public static Element FindElement(IEngine engine, int dmaId, int elementId)
        {
            var element = engine.FindElement(dmaId, elementId);
            if (element == null || element.IsActive == false)
            {
                throw new InvalidOperationException($"Couldn't find element {dmaId}/{elementId}");
            }
            else
            {
                return element;
            }
        }
    }
}