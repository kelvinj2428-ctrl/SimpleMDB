import { $, apiFetch, renderStatus, clearChildren, getQueryParam } from '/scripts/common.js';

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
		return;
	}

	// Load actors in this movie
	const actorsStatusEl = $('#actors-status');
	const actorsListEl = $('#actors-list');
	const tpl = $('#actor-item');

	try {
		const payload = await apiFetch(`/movies/${encodeURIComponent(id)}/actors?page=1&size=20`);
		const items = Array.isArray(payload) ? payload : (payload.data || []);
		clearChildren(actorsListEl);

		if (items.length === 0) {
			renderStatus(actorsStatusEl, 'warn', 'No actors found for this movie.');
		} else {
			renderStatus(actorsStatusEl, '', '');
			for (const a of items) {
				const frag = tpl.content.cloneNode(true);
				const root = frag.querySelector('.card');
				const name = (a.firstName && a.lastName)
					? `${a.firstName} ${a.lastName}`
					: `Actor #${a.actorId ?? a.id}`;
				root.querySelector('.name').textContent = name;
				root.querySelector('.role-badge').textContent = a.role || '';
				root.querySelector('.btn-view').href = `/actors/view.html?id=${encodeURIComponent(a.actorId ?? a.id)}`;
				actorsListEl.appendChild(frag);
			}
		}
	} catch (err) {
		renderStatus(actorsStatusEl, 'warn', `Could not load actors: ${err.message}`);
	}
})();
