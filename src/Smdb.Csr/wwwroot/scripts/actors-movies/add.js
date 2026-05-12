import { $, apiFetch, renderStatus } from '/scripts/common.js';

(async function initActorMovieAdd() {
	const form = $('#am-form');
	const statusEl = $('#status');

	form.addEventListener('submit', async (ev) => {
		ev.preventDefault();
		const payload = {
			actorId: Number(form.actorId.value),
			movieId: Number(form.movieId.value),
			role: form.role.value.trim()
		};
		try {
			const created = await apiFetch('/actors-movies', { method: 'POST', body: JSON.stringify(payload) });
			renderStatus(statusEl, 'ok', `Created relationship #${created.id} (Actor ${created.actorId} → Movie ${created.movieId}).`);
			form.reset();
		} catch (err) {
			renderStatus(statusEl, 'err', `Create failed: ${err.message}`);
		}
	});
})();
