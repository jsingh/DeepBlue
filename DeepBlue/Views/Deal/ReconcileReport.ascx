<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Capital Calls From Investments</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Capital Calls From Investments
				</div>
			</div>
		</div>
		<div class="recon-detail" id="RGUFCC" style="display: block;" issearch="true">
			{{tmpl(UFCCItems) "#ReconcileGridTemplate"}}
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Distributions/Sales Of Investment</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Distributions/Sales Of Investment
				</div>
			</div>
		</div>
		<div class="recon-detail" id="RGUFCD" style="display: block;" issearch="true">
			{{tmpl(UFCDItems) "#ReconcileGridTemplate"}}
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Capital Call To Investor</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Capital Call To Investor
				</div>
			</div>
		</div>
		<div class="recon-detail" id="RGCC" style="display: block;" issearch="true">
			{{tmpl(CCItems) "#ReconcileGridTemplate"}}
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Distribution To Investor</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Distribution To Investor
				</div>
			</div>
		</div>
		<div class="recon-detail" id="RGCD" style="display: block;" issearch="true">
			{{tmpl(CDItems) "#ReconcileGridTemplate"}}
		</div>
	</div>
</div>
<div class="line">
</div>
<div class="act-box">
	<div class="group">
		<div class="recon-headerbox" style="display: none;">
			<div class="title">
				<span>Fund Expenses</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Fund Expenses
				</div>
			</div>
		</div>
		<div class="recon-detail" id="RGFE" style="display: block;" issearch="true">
			{{tmpl(FundExpenses) "#ReconcileGridTemplate"}}
		</div>
	</div>
</div>
<div class="line">
</div>