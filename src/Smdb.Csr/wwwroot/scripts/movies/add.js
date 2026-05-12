import { $, apiFetch, renderStatus, captureMovieForm } from '/scripts/common.js';

(async function initMovieAdd() {
	const form = $('#movie-form');
	const statusEl = $('#status');

	form.addEventListener('submit', async (ev) => {
		ev.preventDefault();
		const payload = captureMovieForm(form);

		// ── Frontend validation ───────────────────────────────────────────────
		if (!payload.title) {
			renderStatus(statusEl, 'err', 'Title is required.');
			return;
		}
		if (payload.title.length > 256) {
			renderStatus(statusEl, 'err', 'Title cannot be longer than 256 characters.');
			return;
		}
		if (!payload.year || payload.year < 1888 || payload.year > new Date().getFullYear()) {
			renderStatus(statusEl, 'err', `Year must be between 1888 and ${new Date().getFullYear()}.`);
			return;
		}
		if (payload.rating < 0 || payload.rating > 10) {
			renderStatus(statusEl, 'err', 'Rating must be between 0 and 10.');
			return;
		}

		try {
			const created = await apiFetch('/movies', {
				method: 'POST',
				body: JSON.stringify(payload),
			});
			renderStatus(statusEl, 'ok',
				`Created movie #${created.id} "${created.title}" (${created.year}).`);
			form.reset();
		} catch (err) {
			renderStatus(statusEl, 'err', `Create failed: ${err.message}`);
		}
	});
})();
