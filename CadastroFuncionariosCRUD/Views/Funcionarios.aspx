<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Funcionarios.aspx.vb" Inherits="CadastroFuncionariosCRUD.Funcionarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function showSuccessAlert() {
            Swal.fire(
                'Sucesso!',
                'Funcionario cadastrado com sucesso!',
                'success'
            )
        }
        function showDeletedSuccessAlert() {
            Swal.fire(
                'Deletado!',
                'Seu registro foi excluído.',
                'success'
            )
        }
        function showEditSuccessAlert() {
            Swal.fire(
                'Sucesso!',
                'Funcionario editado com sucesso!',
                'success'
            )
        }

        //Limpar campos modal quando for fechada após o click do botao "editar"
        $(document).ready(function LimpaModal () {
            $('#modalCadastro').on('hidden.bs.modal', function LimpaModal () {
                $('#MainContent_txtNome').val('');
                $('#MainContent_txtDataNasc').val('');
                $('#MainContent_txtEmail').val('');
                $('#MainContent_txtTelefone').val('');               
                $('#MainContent_buttonAdd').val('Adicionar');
            });
        });

        $(document).ready(function () {
            // Aplica a máscara ao campo txtTelefone
            $('#MainContent_txtTelefone').mask('(00) 00000-0000');
        });

        function validateForm() {
            var nome = document.getElementById('<%= txtNome.ClientID %>').value;
                var dataNasc = document.getElementById('<%= txtDataNasc.ClientID %>').value;
                if (nome == "" || dataNasc == "") {
                    alert("Os campos Nome e Data Nascimento são obrigatórios.");
                    return false;
                }
                return true;
            }

    </script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grd" AutoGenerateColumns="false" runat="server" CssClass="table table-striped" DataKeyNames="ID">
                <Columns>
                    <asp:BoundField DataField="nome" HeaderText="Nome" />
                    <asp:BoundField DataField="dataNascimento" HeaderText="Data de Nascimento"  DataFormatString="{0:dd/MM/yyyy}" />
                    <asp:BoundField DataField="email" HeaderText="Email" />
                    <asp:BoundField DataField="telefone" HeaderText="Telefone" />
                    <asp:TemplateField HeaderText="Opções">
                        <ItemTemplate>
                            <asp:Button ID="btnEditar" runat="server" Text="Editar" CssClass="btn btn-warning float-right" OnClick="btn_Editar" CommandArgument='<%# Eval("id") %>' />
                            <asp:Button ID="btnDeletar" runat="server" Text="Deletar" CssClass="btn btn-danger float-right" OnClick="btnDeletar_Click" CommandArgument='<%# Eval("id") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView> 
                <asp:Button ID="btnAdicionar" runat="server" Text="Adicionar" CssClass="btn btn-primary" />
        </ContentTemplate>
    </asp:UpdatePanel>


    <%--MODAL PARA ABRIR FORMULARIO DE CADASTRO--%>
    <div class="modal fade" id="modalCadastro">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title">Cadastro</h4>
                    <button type="button" class="close btn btn-danger" data-bs-dismiss="modal" data-dismiss="modal">&times;</button>
                </div>

                <div class="modal-body">
                    <div class="form-group">
                        <label>Nome:</label>
                        <asp:TextBox ID="txtNome" runat="server" CssClass="form-control"></asp:TextBox>                       
                    </div>
                    <div class="form-group">
                        <label>Data Nascimento:</label>
                        <asp:TextBox ID="txtDataNasc" runat="server" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>E-mail:</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>Telefone:</label>
                        <asp:TextBox ID="txtTelefone" runat="server" TextMode="SingleLine" CssClass="form-control"></asp:TextBox>
                    </div>

                    <div class="row">
                        <div class="col-6 d-flex mt-3 d-flex justify-content-start">
                            <asp:Button ID="buttonAdd" runat="server" Text="Adicionar" CssClass="btn btn-primary" OnClientClick="return validateForm();" />
                        </div>
                        <div  class="col-6 d-flex mt-3 d-flex justify-content-end">
                            <button type="button" id="buttonCancelar" data-bs-dismiss="modal" class="btn btn-danger">Cancelar</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
