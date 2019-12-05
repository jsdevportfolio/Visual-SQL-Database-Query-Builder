<%@ Page Title="" Language="C#" MasterPageFile="~/DataManager.Master" AutoEventWireup="true" CodeBehind="DataBuilder.aspx.cs" Inherits="NSLookup.WebForm1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
    
    <div id="result"></div>

<script>
    function store() {
        // Check browser support
        if (typeof (Storage) !== "undefined") {
            // Store
            localStorage.setItem("table.csv", "Generated Table");
            // Retrieve
            localStorage.getItem("table.csv");
        }
        else
        {
            document.getElementById("result").innerHTML = "Sorry, your browser does not support Web Storage...";
        }
    }
</script>

    <style>
        .jumbotron
        {
            background-color:#f4511e;
            color:#fff;
        }
            </style>
<body>
    <br />
    <br />
   <br />
       <div class="col-sm-12">
       <center>
        <!-- Modal -->
<div id="myModal" class="modal fade" role="dialog">
  <div class="modal-dialog">

    <!-- Modal content-->
    <div class="modal-content">
      <div class="modal-header">
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        <h4 class="modal-title">Connect To A Server</h4>
      </div>
      <div class="modal-body">
            <br />
            <h1>SERVER</h1>
            <p>Authentication</p>
            <br />
            <asp:TextBox ID="TextBox3" runat="server" placeholder="Server Name" type="text" class="form-control"></asp:TextBox>
            <br />
            <asp:TextBox ID="TextBox1" runat="server" placeholder="Username" type="text" class="form-control"></asp:TextBox>
            <br />
            <asp:TextBox ID="TextBox2" runat="server" placeholder="Password" type="password" class="form-control"></asp:TextBox>
            <br />
            <asp:CheckBox ID="CheckBox1" runat="server" OnCheckedChanged="CheckBox1_CheckedChanged" Text="Use Current Windows User Credentials" />
            <br />
            <br />
            <asp:Button ID="Button4" runat="server" data-toggle="modal" data-target="#myModal" class="btn btn-info btn-primary" Text="Connect to Server" OnClick="Button4_Click"></asp:Button>
            <br />
            <br />
        </div>
      <div class="modal-footer">
        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
      </div>
    </div>

  </div>
</div>
       </div>
     
    <div class="col-sm-12" id="navigator">
         <ul class="nav nav-tabs">
            <li class="active"><a data-toggle="tab" href="#builder2"> <span class="glyphicon glyphicon-cog" aria-hidden="true"></span> Table Builder</a></li>
            <li><a data-toggle="tab" href="#home"> <span class="glyphicon glyphicon-th-list" aria-hidden="true"></span> Generated Table</a></li>
            <li><a data-toggle="tab" href="#menu1"> <span class="glyphicon glyphicon-question-sign" aria-hidden="true"></span> Generated Query</a></li>
            <li><a data-toggle="tab" href="#Data"> <span class="glyphicon glyphicon-file" aria-hidden="true"></span> Generated Files</a></li>
         </ul>
    </div>
    
    <div class="col-sm-12">
        
        <div class="tab-content">
            <div id="builder2" class="tab-pane in active">
                <div class="col-sm-12">
                    
                    <center>
                    <h1><asp:Label ID="Label4" runat="server"></asp:Label></h1>
                    <div class="jumbotron text-center">
                        <button type="button" class="btn btn-info btn-lg" data-target="#myModal" data-toggle="modal"><span class="glyphicon glyphicon-link" aria-hidden="true"></span> Connect To Server</button>
                        
                    </div>
                     </center>
                </div>
                <div class="col-sm-12" id="builder">
                    <center>
        <div class="col-sm-4">
            <br />
            <h1>DATABASE</h1>
            <br />
            <!--<iframe name="frame1"></iframe>-->
            <asp:RadioButtonList ID="RadioButtonList2" runat="server" class="breadcrumb" TextAlign="Right" RepeatDirection="Horizontal" RepeatColumns="1" Width="100%" BorderStyle="None"></asp:RadioButtonList>    
            
            <br />
            <br />
            <asp:Button ID="Button3" runat="server" class="btn btn-info btn-primary" OnClick="Button3_Click" Text="Connect to Database" />
            <br />
            <br />
        </div>
            
        <div class="col-sm-4">
            <br />
            <h1>TABLES / VIEWS</h1>
            <br />
            <asp:RadioButtonList ID="RadioButtonList1" runat="server" class="breadcrumb" TextAlign="Right" RepeatDirection="Horizontal" RepeatColumns="1" Width="100%" BorderStyle="None"></asp:RadioButtonList>
           
            <br />
            <br />
            <asp:Button ID="Button1" runat="server" class="btn btn-info btn-primary" OnClick="Button1_Click" Text="Connect to Table" data-toggle="collapse" data-target="#demo"/>
            <br />
            <br />
        </div>

        <div class="col-sm-4"> 
            <br />
            <h1>FIELDS</h1>
            <br />
            <asp:CheckBoxList ID="CheckBoxList1" runat="server" class="breadcrumb" TextAlign="Right" Width="100%" RepeatColumns="1" RepeatDirection="Horizontal" BorderStyle="None">
            </asp:CheckBoxList>
            <br />
            <!--
            <asp:TextBox ID="TextBox5" runat="server"   placeholder="Conditional Statement - WHERE, BETWEEN, ORDER BY, ETC..." type="text" class="form-control">
                       </asp:TextBox>
            -->
            <br />
       
            <asp:Button ID="Button2" runat="server" class="btn btn-info btn-primary" OnClick="Button2_Click" Text="Connect to Fields" />
            <br />
            <br />
            
        </div>

                
                        
                
                        <br />
                        </center>
                </div>
            </div>
            <div id="home" class="tab-pane fade">
                <center><h1><asp:Label ID="Label1" runat="server">Table Not Generated</asp:Label></h1></center>
                    <asp:Table ID="Table1" runat="server" CssClass="table table-responsive table-bordered table-condensed table-hover"></asp:Table>
            </div>
            <div id="menu1" class="tab-pane fade">
                <br />
                <asp:TextBox ID="TextBox4" runat="server" TextMode="MultiLine" Width="100%" placeholder="Generated Query String" Height="500px" ReadOnly="True" class="form-control"></asp:TextBox>
            </div>
            <div id="Data" class="tab-pane fade">
                <br />
                 <center>
                     <div class="col-sm-12">
                         <br />
                <center>
                    
                    <div class="row">
                        <div class="col-sm-6 col-md-6">
                            <div class="thumbnail">
                                <img src="C:\Users\JS\Documents\Source Buffet.png" alt="...">
                                    <div class="caption">
                                        <h3>CSV</h3>
                                        <p>...</p>
                                        <asp:Button ID="Button5" runat="server" class="btn btn-info btn-primary" OnClick="Button5_Click1" Text="Retrieve Generated .CSV" />   
                                    </div>
                            </div>
                        </div>
                        <div class="col-sm-6 col-md-6">
                            <div class="thumbnail">
                                <img src="C:\Users\JS\Documents\Source Buffet.png" alt="...">
                                    <div class="caption">
                                        <h3>SQL</h3>
                                        <p>...</p>
                                       <asp:Button ID="Button8" runat="server" class="btn btn-info" type="button" Text="Retrieve Generated .SQL" OnClick="Button8_Click1"></asp:Button>
                                    </div>
                            </div>
                        </div>
                    </div>
                
                         <br />   
                    <br />
                    <asp:Label ID="Label6" runat="server" class="alert alert-success col-sm-12" role="alert" Text=""></asp:Label>
                    <br />
                    <br />   
                    <br />
                    <asp:Label ID="Label7" runat="server" class="alert alert-success col-sm-12" role="alert" Text=".SQL File Generated and sent to C:\Generated Data"></asp:Label>
                    <br />
                </center></center>
                <br /> 
                <asp:Label ID="Label2" runat="server" Text=""></asp:Label>
                    
                    
                    
                    
                    </div>
                 </center>
            </div>
            
        </div>
        </div>
            
        
        
    
    
        
    
    
    
            
        
        
    
    
        
    
</body>
</html>

</asp:Content>
