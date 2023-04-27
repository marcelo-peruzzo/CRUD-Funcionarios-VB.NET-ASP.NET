<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/Site.Master" CodeBehind="Funcionarios.aspx.vb" Inherits="CadastroFuncionariosCRUD.Funcionarios" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>

        function showSuccessAlert() {
            Swal.fire(
                'Sucesso!',
                'Pessoa cadastrada com sucesso!',
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
                'Pessoa editada com sucesso!',
                'success'
            )
        }

    </script>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:GridView ID="grd" AutoGenerateColumns="false" runat="server" CssClass="table table-striped" DataKeyNames="ID">
                <Columns>
                    <asp:BoundField DataField="nome" HeaderText="Nome" />
                    <asp:BoundField DataField="dataNascimento" HeaderText="Data de Nascimento" />
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
    <asp:Button ID="btnAtualizar" runat="server" Text="Atualizar" CssClass="btn btn-success" />
    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />

    <%--MODAL PARA ABRIR FORMULARIO DE CADASTRO--%>
    <div class="modal fade" id="modalCadastro">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <h4 class="modal-title">Cadastro</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
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
                        <asp:TextBox ID="txtTelefone" runat="server" TextMode="Number" CssClass="form-control"></asp:TextBox>
                    </div>
<%--                    <div class="form-group my-2">
                        <asp:CheckBox ID="chkAtivo" runat="server" Text="Ativo" />
                    </div>--%>

                    <div class="row">
                        <div class="col-6 d-flex d-flex justify-content-start">
                            <asp:Button ID="ButtonAdd" runat="server" Text="Adicionar" CssClass="btn btn-primary" />
                        </div>
                        <div class="col-6 d-flex d-flex justify-content-start">
                            <asp:Button ID="buttonCancelar" runat="server" Text="Cancelar" CssClass="btn btn-danger" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
