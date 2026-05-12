import { $, apiFetch, renderStatus, getQueryParam, captureMovieForm } from '/scripts/common.js';

(async function initMovieEdit() {
	const id = getQueryParam('id');
	const form = $('#movie-form');
	const statusEl = $('#status');

	// ── Guard: missing id ─────────────────────────────────────────────────────
	if (!id) {
		renderStatus(statusEl, 'err', 'Missing ?id in URL.');
		form.querySelectorAll('input,textarea,button,select').forEach(el => el.disabled = true);
		return;
	}

	// ── Load existing movie data ──────────────────────────────────────────────
	try {
		const m = await apiFetch(`/movies/${encodeURIComponent(id)}`);
		form.title.value = m.title ?? '';
		form.year.value = m.year ?? '';
		form.genre.value = m.genre ?? '';
		form.rating.value = m.rating ?? 0;
		form.description.value = m.description ?? '';
		renderStatus(statusEl, 'ok', 'Loaded movie. You can edit and save.');
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to load data: ${err.message}`);
		return;
	}

	// ── Handle form submission ────────────────────────────────────────────────
	form.addEventListener('submit', async (ev) => {
		ev.preventDefault();
		const payload = captureMovieForm(form);

		// ── Frontend validation ─────────────────────────────────────────────
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
			const updated = await apiFetch(`/movies/${encodeURIComponent(id)}`, {
				method: 'PUT',
				body: JSON.stringify(payload),
			});
			renderStatus(statusEl, 'ok',
				`Updated movie #${updated.id} "${updated.title}" (${updated.year}).`);
		} catch (err) {
			renderStatus(statusEl, 'err', `Update failed: ${err.message}`);
		}
	});
})();
