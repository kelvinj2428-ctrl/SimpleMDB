import { $, apiFetch, renderStatus, clearChildren, getQueryParam } from '/scripts/common.js';

(async function initActorMoviesIndex() {
	const page = Math.max(1, Number(getQueryParam('page') || localStorage.getItem('am.page') || '1'));
	const size = Math.min(100, Math.max(1, Number(getQueryParam('size') || localStorage.getItem('am.size') || '9')));
	localStorage.setItem('am.page', page);
	localStorage.setItem('am.size', size);

	const listEl = $('#am-list');
	const statusEl = $('#status');
	const tpl = $('#am-card');

	try {
		const payload = await apiFetch(`/actors-movies?page=${page}&size=${size}`);
		const items = Array.isArray(payload) ? payload : (payload.data || []);
		clearChildren(listEl);

		if (items.length === 0) {
			renderStatus(statusEl, 'warn', 'No actor-movie relationships found for this page.');
		} else {
			renderStatus(statusEl, '', '');
			for (const am of items) {
				const frag = tpl.content.cloneNode(true);
				const root = frag.querySelector('.card');
				root.querySelector('.role-name').textContent = am.role || `Relationship #${am.id}`;
				root.querySelector('.ids').textContent = `Actor ID: ${am.actorId} | Movie ID: ${am.movieId}`;
				root.querySelector('.btn-view').href = `/actors-movies/view.html?id=${encodeURIComponent(am.id)}`;
				root.querySelector('.btn-edit').href = `/actors-movies/edit.html?id=${encodeURIComponent(am.id)}`;
				root.querySelector('.btn-delete').dataset.id = am.id;
				listEl.appendChild(frag);
			}
		}

		listEl.addEventListener('click', async (ev) => {
			const btn = ev.target.closest('button.btn-delete[data-id]');
			if (!btn) return;
			const id = btn.dataset.id;
			if (!confirm('Delete this relationship? This cannot be undone.')) return;
			try {
				await apiFetch(`/actors-movies/${encodeURIComponent(id)}`, { method: 'DELETE' });
				renderStatus(statusEl, 'ok', `Relationship ${id} deleted.`);
				setTimeout(() => location.reload(), 2000);
			} catch (err) {
				renderStatus(statusEl, 'err', `Delete failed: ${err.message}`);
			}
		});

		const sizeSelect = document.getElementById('page-size');
		const pageSizes = [3, 6, 9, 12, 15];
		for (const s of pageSizes) {
			const opt = document.createElement('option');
			opt.value = s;
			opt.textContent = String(s);
			opt.selected = (size == s);
			sizeSelect.appendChild(opt);
		}
		sizeSelect.addEventListener('change', () => {
			const params = new URLSearchParams(window.location.search);
			params.set('page', 1);
			params.set('size', sizeSelect.value);
			localStorage.setItem('am.page', 1);
			localStorage.setItem('am.size', sizeSelect.value);
			window.location.href = `${window.location.pathname}?${params.toString()}`;
		});

		$('#page-num').textContent = `Page ${page}`;
		const totalPages = payload.meta ? payload.meta.totalPages : 1;
		const firstPage = page <= 1;
		const lastPage = page >= totalPages;
		$('#first').href = `?page=1&size=${size}`;
		$('#prev').href = `?page=${page - 1}&size=${size}`;
		$('#next').href = `?page=${page + 1}&size=${size}`;
		$('#last').href = `?page=${totalPages}&size=${size}`;
		$('#first').classList.toggle('disabled', firstPage);
		$('#prev').classList.toggle('disabled', firstPage);
		$('#next').classList.toggle('disabled', lastPage);
		$('#last').classList.toggle('disabled', lastPage);
		$('#first').setAttribute('onclick', `return ${!firstPage};`);
		$('#prev').setAttribute('onclick', `return ${!firstPage};`);
		$('#next').setAttribute('onclick', `return ${!lastPage};`);
		$('#last').setAttribute('onclick', `return ${!lastPage};`);
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to fetch actor-movies: ${err.message}`);
	}
})();
