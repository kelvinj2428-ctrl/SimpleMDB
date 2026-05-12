import { $, apiFetch, renderStatus, getQueryParam } from '/scripts/common.js';

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
	}
})();
