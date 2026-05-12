import { $, apiFetch, renderStatus, getQueryParam } from '/scripts/common.js';

(async function initActorMovieEdit() {
	const id = getQueryParam('id');
	const form = $('#am-form');
	const statusEl = $('#status');

	if (!id) {
		renderStatus(statusEl, 'err', 'Missing ?id in URL.');
		form.querySelectorAll('input,textarea,button,select').forEach(el => el.disabled = true);
		return;
	}

	try {
		const am = await apiFetch(`/actors-movies/${encodeURIComponent(id)}`);
		form.actorId.value = am.actorId ?? '';
		form.movieId.value = am.movieId ?? '';
		form.role.value = am.role ?? '';
		renderStatus(statusEl, 'ok', 'Loaded relationship. You can edit and save.');
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to load data: ${err.message}`);
		return;
	}

	form.addEventListener('submit', async (ev) => {
		ev.preventDefault();
		const payload = {
			actorId: Number(form.actorId.value),
			movieId: Number(form.movieId.value),
			role: form.role.value.trim()
		};
		try {
			const updated = await apiFetch(`/actors-movies/${encodeURIComponent(id)}`, {
				method: 'PUT',
				body: JSON.stringify(payload),
			});
			renderStatus(statusEl, 'ok', `Updated relationship #${updated.id}.`);
		} catch (err) {
			renderStatus(statusEl, 'err', `Update failed: ${err.message}`);
		}
	});
})();
