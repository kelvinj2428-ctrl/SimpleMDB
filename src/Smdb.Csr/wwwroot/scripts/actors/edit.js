import { $, apiFetch, renderStatus, getQueryParam } from '/scripts/common.js';

(async function initActorEdit() {
	const id = getQueryParam('id');
	const form = $('#actor-form');
	const statusEl = $('#status');

	if (!id) {
		renderStatus(statusEl, 'err', 'Missing ?id in URL.');
		form.querySelectorAll('input,textarea,button,select').forEach(
			el => el.disabled = true);
		return;
	}

	// Populate form with existing data
	try {
		const a = await apiFetch(`/actors/${encodeURIComponent(id)}`);
		form.firstName.value = a.firstName ?? '';
		form.lastName.value = a.lastName ?? '';
		form.rating.value = a.rating ?? 0;
		form.bio.value = a.bio ?? '';
		renderStatus(statusEl, 'ok', 'Loaded actor. You can edit and save.');
	} catch (err) {
		renderStatus(statusEl, 'err', `Failed to load data: ${err.message}`);
		return;
	}

	// Handle form submit
	form.addEventListener('submit', async (ev) => {
		ev.preventDefault();

		const payload = {
			firstName: form.firstName.value.trim(),
			lastName: form.lastName.value.trim(),
			rating: Number(form.rating.value),
			bio: form.bio.value.trim()
		};

		try {
			const updated = await apiFetch(`/actors/${encodeURIComponent(id)}`, {
				method: 'PUT',
				body: JSON.stringify(payload),
			});
			renderStatus(statusEl, 'ok',
				`Updated actor #${updated.id} "${updated.firstName} ${updated.lastName}".`);
		} catch (err) {
			renderStatus(statusEl, 'err', `Update failed: ${err.message}`);
		}
	});
})();
