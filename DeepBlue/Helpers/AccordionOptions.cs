using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeepBlue.Helpers {
	public class AccordionOptions {
		public AccordionOptions() {
			Disabled = false;
			Active = 1;
			Collapsible = true;
			AutoHeight = false;
		}
		public bool Disabled { get; set; }
		public int Active { get; set; }
		public bool Collapsible { get; set; }
		public bool AutoHeight { get; set; }
	}
}