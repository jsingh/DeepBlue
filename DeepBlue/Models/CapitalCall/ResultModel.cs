﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace DeepBlue.Models.CapitalCall {
	public class ResultModel {
		public ResultModel(){
			Result = string.Empty;
		}

		public string Result { get; set; }
	}
}