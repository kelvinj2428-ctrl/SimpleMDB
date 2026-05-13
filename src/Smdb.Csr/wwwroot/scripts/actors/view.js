import { $, apiFetch, renderStatus, clearChildren, getQueryParam } from '/scripts/common.js';

(async function initActorView() {
	const id = getQueryParam('id');
	const statusEl = $('#status');

	if (!id) return renderStatus(statusEl, 'err', 'Missing ?id in URL.');

	try {
		const a = await apiFetch(`/actors/${encodeURIComponent(id)}`);
		$('#actor-id').textContent = a.id;
		$('#actor-firstName').textContent = a.firstName ?? '—';
		$('#actor-lastName').textContent = a.lastName ?? '—';
		$('#actor-rating').textContent = a.rating != null ? a.rating : '—';
		$('#actor-bio').textContent = a.bio || '—';
		$('#edit-link').href = `/actors/edit.html?id=${encodeURIComponent(a.id)}`;
		renderStatus(statusEl, 'ok', 'Actor loaded successfully.');
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to load actor ${id}: ${err.message}`);
		return;
	}

	// Load movies by this actor
	const moviesStatusEl = $('#movies-status');
	const moviesListEl = $('#movies-list');
	const tpl = $('#movie-item');

	try {
		const payload = await apiFetch(`/actors/${encodeURIComponent(id)}/movies?page=1&size=20`);
		const items = Array.isArray(payload) ? payload : (payload.data || []);
		clearChildren(moviesListEl);

		if (items.length === 0) {
			renderStatus(moviesStatusEl, 'warn', 'No movies found for this actor.');
		} else {
			renderStatus(moviesStatusEl, '', '');
			for (const m of items) {
				const frag = tpl.content.cloneNode(true);
				const root = frag.querySelector('.card');
				root.querySelector('.title').textContent = m.title ?? `Movie #${m.movieId ?? m.id}`;
				root.querySelector('.year').textContent = m.year ? String(m.year) : '';
				root.querySelector('.role-text').textContent = m.role ? `Role: ${m.role}` : '';
				root.querySelector('.btn-view').href = `/movies/view.html?id=${encodeURIComponent(m.movieId ?? m.id)}`;
				moviesListEl.appendChild(frag);
			}
		}
	} catch (err) {
		renderStatus(moviesStatusEl, 'warn', `Could not load movies: ${err.message}`);
	}
})();
