const API = "http://localhost:5094/api/Produtos";
const token = localStorage.getItem("token");
const paginaAtual = window.location.pathname;

// Redirecionamento se não estiver logado
if (!token && !paginaAtual.includes("login.html")) {
    window.location.href = "login.html";
}

let editandoId = null;
let produtosCache = [];
let carrinho = [];

// --- FUNÇÃO DE LOGIN ---
async function login() {
    const email = document.getElementById("email").value;
    const senha = document.getElementById("senha").value;

    if (!email || !senha) {
        alert("Preencha todos os campos!");
        return;
    }

    try {
        const response = await fetch("http://localhost:5094/api/Auth/login", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify({ email, senha })
        });

        if (response.ok) {
            const data = await response.json();
            localStorage.setItem("token", data.token);
            window.location.href = "index.html";
        } else {
            alert("Email ou senha inválidos!");
        }
    } catch (error) {
        console.error("Erro no login:", error);
        alert("Erro ao conectar com API!");
    }
}

// --- FUNÇÕES DE PRODUTOS ---
async function listarProdutos() {
    try {
        const response = await fetch(API, {
            headers: { "Authorization": `Bearer ${token}` }
        });

        if (!response.ok) throw new Error("Erro ao buscar produtos");

        const produtos = await response.json();
        produtosCache = produtos;

        const tabela = document.getElementById("tabelaProdutos");
        if (tabela) {
            tabela.innerHTML = "";
            produtos.forEach(produto => {
                tabela.innerHTML += `
                    <tr>
                        <td>${produto.id}</td>
                        <td>${produto.nome}</td>
                        <td>R$ ${Number(produto.preco).toFixed(2)}</td>
                        <td>${produto.categoria}</td>
                        <td>
                            <button class="btn btn-warning btn-sm" onclick="editarProduto(${produto.id}, '${produto.nome}', ${produto.preco}, '${produto.categoria}')">Editar</button>
                            <button class="btn btn-danger btn-sm" onclick="deletarProduto(${produto.id})">Excluir</button>
                        </td>
                    </tr>`;
            });
        }
        mostrarCardapio(produtos);
    } catch (error) {
        console.error("Erro ao listar produtos:", error);
    }
}

function mostrarCardapio(produtos) {
    const cardapio = document.getElementById("cardapio");
    if (!cardapio) return;

    cardapio.innerHTML = "";
    produtos.forEach(produto => {
        const col = document.createElement("div");
        col.className = "col-md-4 mb-4";
        col.innerHTML = `
            <div class="card shadow h-100">
                <div class="card-body">
                    <h4 class="card-title">${produto.nome}</h4>
                    <p class="card-text">Categoria: ${produto.categoria}</p>
                    <h3 class="text-success">R$ ${Number(produto.preco).toFixed(2)}</h3>
                    <button class="btn btn-primary w-100 mt-3 btn-add">Adicionar ao Carrinho</button>
                </div>
            </div>`;

        col.querySelector(".btn-add").addEventListener("click", () => {
            adicionarCarrinho(produto.id);
        });

        cardapio.appendChild(col);
    });
}

async function salvarProduto() {
    const nome = document.getElementById("nome").value;
    const preco = parseFloat(document.getElementById("preco").value);
    const categoria = document.getElementById("categoria").value;

    if (!nome || isNaN(preco)) {
        alert("Preencha os campos corretamente!");
        return;
    }

    const produto = { nome, preco, categoria };

    try {
        const method = editandoId ? "PUT" : "POST";
        const url = editandoId ? `${API}/${editandoId}` : API;

        const response = await fetch(url, {
            method,
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(produto)
        });

        if (response.ok) {
            editandoId = null;
            limparCampos();
            listarProdutos();
        } else {
            alert("Erro ao salvar produto!");
        }
    } catch (error) {
        console.error("Erro ao salvar:", error);
        alert("Erro ao conectar com API!");
    }
}

function editarProduto(id, nome, preco, categoria) {
    document.getElementById("nome").value = nome;
    document.getElementById("preco").value = preco;
    document.getElementById("categoria").value = categoria;
    editandoId = id;
}

async function deletarProduto(id) {
    if (!confirm("Deseja realmente excluir este produto?")) return;
    try {
        const response = await fetch(`${API}/${id}`, {
            method: "DELETE",
            headers: { "Authorization": `Bearer ${token}` }
        });
        if (response.ok) {
            listarProdutos();
        } else {
            alert("Erro ao excluir produto!");
        }
    } catch (error) {
        console.error("Erro ao deletar:", error);
    }
}

function limparCampos() {
    document.getElementById("nome").value = "";
    document.getElementById("preco").value = "";
    document.getElementById("categoria").value = "Lanche";
}

function logout() {
    localStorage.removeItem("token");
    window.location.href = "login.html";
}

// --- FUNÇÕES DO CARRINHO ---
function adicionarCarrinho(id) {
    const produto = produtosCache.find(p => p.id === id);

    if (!produto) {
        console.error("Produto não encontrado no cache:", id);
        return;
    }

    const itemExistente = carrinho.find(item => item.id === produto.id);

    if (itemExistente) {
        itemExistente.quantidade++;
    } else {
        carrinho.push({
            id: produto.id,
            nome: produto.nome,
            preco: Number(produto.preco),
            quantidade: 1
        });
    }
    atualizarCarrinho();
}

function removerCarrinho(id) {
    carrinho = carrinho.filter(item => item.id !== id);
    atualizarCarrinho();
}

function atualizarCarrinho() {
    const carrinhoItens = document.getElementById("carrinhoItens");
    const totalCarrinho = document.getElementById("totalCarrinho");

    if (!carrinhoItens || !totalCarrinho) return;

    carrinhoItens.innerHTML = "";
    let total = 0;

    carrinho.forEach(item => {
        const preco = Number(item.preco) || 0;
        const subtotal = preco * item.quantidade;
        total += subtotal;

        carrinhoItens.innerHTML += `
            <div class="d-flex justify-content-between align-items-center border-bottom py-3">
                <div>
                    <h5>${item.nome}</h5>
                    <p>R$ ${preco.toFixed(2)} x ${item.quantidade}</p>
                    <p><strong>Subtotal: R$ ${subtotal.toFixed(2)}</strong></p>
                </div>
                <button class="btn btn-danger" onclick="removerCarrinho(${item.id})">Remover</button>
            </div>`;
    });

    totalCarrinho.innerText = total.toFixed(2);
}

async function finalizarCompra() {
    if (carrinho.length === 0) {
        alert("Carrinho vazio!");
        return;
    }

    const pedido = {
        itens: carrinho.map(item => ({
            produtoId: item.id,
            quantidade: item.quantidade
        }))
    };

    try {
        const response = await fetch("http://localhost:5094/api/Pedidos", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${token}`
            },
            body: JSON.stringify(pedido)
        });

        if (response.ok) {
            alert("Pedido finalizado!");
            carrinho = [];
            atualizarCarrinho();
        } else {
            alert("Erro ao finalizar o pedido!");
        }
    } catch (error) {
        console.error("Erro ao finalizar compra:", error);
        alert("Erro ao conectar com API!");
    }
}

// Inicialização automática
if (!paginaAtual.includes("login.html")) {
    listarProdutos();
}