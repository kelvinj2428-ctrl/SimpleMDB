import { $, apiFetch, renderStatus, getQueryParam } from '/scripts/common.js';

(async function initUserView() {
	const id = getQueryParam('id');
	const statusEl = $('#status');
	if (!id) return renderStatus(statusEl, 'err', 'Missing ?id in URL.');

	try {
		const u = await apiFetch(`/users/${encodeURIComponent(id)}`);
		$('#user-id').textContent = u.id;
		$('#user-username').textContent = u.username ?? '—';
		$('#user-role').textContent = u.role ?? '—';
		$('#edit-link').href = `/users/edit.html?id=${encodeURIComponent(u.id)}`;
		renderStatus(statusEl, 'ok', 'User loaded successfully.');
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to load user ${id}: ${err.message}`);
	}
})();
