**Pacotes do BackEnd:**



Mirosoft.NET.Sdk.Web - 10.0

Microsoft.AspNetCore.Authentication.JwtBearer - 10.0.7

Microsoft.EntityFrameworkCore.Design - 8.0.20

Microsoft.EntityFrameworkCore.Tools - 8.0.20

Microsoft.OpenApi - 1.6.22

Pomelo.EntityFrameworkCore.MySql - 8.0.2

Swashbuckle.AspNetCore - 6.6.2



**FrontEnd - HTML, CSS e JS:**



Bootstrap: 5.3.3 (via CDN)

JavaScript: Vanilla JS





**Adicionar uma Nova Migration (se houver mudanças no modelo):**



Se você fez alterações nos modelos (Models/Produto.cs, Models/Pedido.cs, etc.), você precisará criar uma nova migration. Substitua NomeDaSuaMigration por um nome descritivo.



dotnet ef migrations add NomeDaSuaMigration





**Atualizar o Banco de Dados:** 



Este comando aplicará todas as migrations pendentes ao seu banco de dados, criando-o se não existir ou atualizando seu esquema.



dotnet ef database update





**Funcionalidades:**



• Autenticação: Login de usuários.



• Gerenciamento de Produtos: Cadastro, edição, exclusão e listagem de produtos.



• Cardápio: Exibição dos produtos disponíveis para compra.



• Carrinho de Compras: Adição, remoção e cálculo de subtotal/total de itens.



• Finalização de Pedido: Envio do pedido para a API.













