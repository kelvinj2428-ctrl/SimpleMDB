import { $, apiFetch, renderStatus } from '/scripts/common.js';

(async function initUserAdd() {
	const form = $('#user-form');
	const statusEl = $('#status');

	form.addEventListener('submit', async (ev) => {
		ev.preventDefault();
		const payload = {
			username: form.username.value.trim(),
			password: form.password.value,
			role: form.role.value
		};
		try {
			const created = await apiFetch('/users', { method: 'POST', body: JSON.stringify(payload) });
			renderStatus(statusEl, 'ok', `Created user #${created.id} "${created.username}".`);
			form.reset();
		} catch (err) {
			renderStatus(statusEl, 'err', `Create failed: ${err.message}`);
		}
	});
})();
