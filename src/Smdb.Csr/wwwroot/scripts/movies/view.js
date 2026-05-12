import { $, apiFetch, renderStatus, getQueryParam } from '/scripts/common.js';

(async function initMovieView() {
	const id = getQueryParam('id');
	const statusEl = $('#status');

	if (!id) {
		renderStatus(statusEl, 'err', 'Missing ?id in URL.');
		return;
	}

	try {
		const m = await apiFetch(`/movies/${encodeURIComponent(id)}`);

		$('#movie-id').textContent = m.id;
		$('#movie-title').textContent = m.title;
		$('#movie-year').textContent = m.year;
		$('#movie-genre').textContent = m.genre || '—';
		$('#movie-rating').textContent = m.rating != null ? m.rating : '—';
		$('#movie-desc').textContent = m.description || '—';

		$('#edit-link').href = `/movies/edit.html?id=${encodeURIComponent(m.id)}`;

		renderStatus(statusEl, 'ok', 'Movie loaded successfully.');
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to load movie ${id}: ${err.message}`);
	}
})();
