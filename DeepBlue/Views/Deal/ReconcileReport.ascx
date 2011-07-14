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
			<div class="rightuarrow">
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
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
			<div class="rightuarrow">
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
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
			<div class="rightuarrow">
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
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
				<span>Capital Call To Investor</span>
			</div>
		</div>
		<div style="display: block;" class="recon-expandheader expandsel">
			<div class="expandbtn">
				<div style="display: block;" class="recon-expandtitle">
					Distribution To Investor
				</div>
			</div>
			<div class="rightuarrow">
			</div>
		</div>
		<div class="recon-detail" style="display: block;" issearch="true">
			{{tmpl(CDItems) "#ReconcileGridTemplate"}}
		</div>
	</div>
</div>
<div class="line">
</div>
