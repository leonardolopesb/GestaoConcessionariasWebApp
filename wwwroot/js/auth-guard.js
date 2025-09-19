// Retorna dados do usuário logado (ou null se não estiver logado)
async function getMe() {
    const r = await fetch('/auth/me', { credentials: 'include' });
    if (!r.ok) return null;
    try { return await r.json(); } catch { return null; }
}

// Vai para a página inicial correta (ou login se não estiver logado)
function go(level) {
    switch ((level || '').toString()) {
        case 'Admin': location.href = '/acessLevel/admin/home.html'; break;
        case 'Gerente': location.href = '/acessLevel/gerente/home.html'; break;
        case 'Vendedor': location.href = '/acessLevel/vendedor/home.html'; break;
        default: location.href = '/login.html'; break;
    }
}

// Em qualquer página protegida: exige login e (opcional) nível.
async function requireAuth(expectedLevel) {
    const me = await getMe();
    if (!me) { location.href = '/login.html'; return; }

    const level = me.AccessLevel || me.accessLevel || '';
    if (expectedLevel && level !== expectedLevel) {
        // se estiver logado mas na página incorreta, manda para a correta
        go(level);
        return;
    }
    return me;
}

// Na tela de login: se já estiver logado, pula para a página inicial correta
async function redirectIfLoggedIn() {
    const me = await getMe();
    if (me) go(me.AccessLevel || me.accessLevel || '');
}
