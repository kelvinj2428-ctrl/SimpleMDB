import { $, apiFetch, renderStatus, clearChildren, getQueryParam } from '/scripts/common.js';

(async function initUsersIndex() {
	const page = Math.max(1, Number(getQueryParam('page') || localStorage.getItem('users.page') || '1'));
	const size = Math.min(100, Math.max(1, Number(getQueryParam('size') || localStorage.getItem('users.size') || '9')));
	localStorage.setItem('users.page', page);
	localStorage.setItem('users.size', size);

	const listEl = $('#user-list');
	const statusEl = $('#status');
	const tpl = $('#user-card');

	try {
		const payload = await apiFetch(`/users?page=${page}&size=${size}`);
		const items = Array.isArray(payload) ? payload : (payload.data || []);
		clearChildren(listEl);

		if (items.length === 0) {
			renderStatus(statusEl, 'warn', 'No users found for this page.');
		} else {
			renderStatus(statusEl, '', '');
			for (const u of items) {
				const frag = tpl.content.cloneNode(true);
				const root = frag.querySelector('.card');
				root.querySelector('.username').textContent = u.username ?? '—';
				root.querySelector('.role').textContent = u.role ?? '—';
				root.querySelector('.btn-view').href = `/users/view.html?id=${encodeURIComponent(u.id)}`;
				root.querySelector('.btn-edit').href = `/users/edit.html?id=${encodeURIComponent(u.id)}`;
				root.querySelector('.btn-delete').dataset.id = u.id;
				listEl.appendChild(frag);
			}
		}

		listEl.addEventListener('click', async (ev) => {
			const btn = ev.target.closest('button.btn-delete[data-id]');
			if (!btn) return;
			const id = btn.dataset.id;
			if (!confirm('Delete this user? This cannot be undone.')) return;
			try {
				await apiFetch(`/users/${encodeURIComponent(id)}`, { method: 'DELETE' });
				renderStatus(statusEl, 'ok', `User ${id} deleted.`);
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
			localStorage.setItem('users.page', 1);
			localStorage.setItem('users.size', sizeSelect.value);
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
		renderStatus(statusEl, 'err', `Failed to fetch users: ${err.message}`);
	}
})();
