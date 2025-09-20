// Retornando dados do usuário logado
async function getUser() {
    const response = await fetch('/auth/me', { credentials: 'include' });

    if (!response.ok)
        return null;

    try { return await response.json(); } catch { return null; }
}

// Se o usuário já estiver logado, pula da tela de login para a página inicial correspondente ao nível de acesso do usuário
function go(nivelAcesso) {
    switch ((nivelAcesso).toString()) {
        case 'Admin':
            location.href = '/acessLevel/admin/home.html';
            break;

        case 'Gerente':
            location.href = '/acessLevel/gerente/home.html';
            break;

        case 'Vendedor':
            location.href = '/acessLevel/vendedor/home.html';
            break;

        default:
            location.href = '/login.html';
            break;
    }
}

async function redirectIfLoggedIn() {
    const user = await getUser();
    if (user)
        go(user.accessLevel);
}

// Exige login e nível de acesso às páginas protegidas
async function requireAuth(nivelAcesso) {
    const user = await getUser();

    if (!user) {
        location.href = '/login.html';
        return;
    }

    const nivelUsuario = user.accessLevel;

    if (nivelAcesso) {
        const ok = Array.isArray(nivelAcesso)
            ? nivelAcesso
            : [nivelAcesso];

        if (!ok.includes(nivelUsuario)) {
            go(nivelUsuario);
            return;
        }
    }

    return user;
}

