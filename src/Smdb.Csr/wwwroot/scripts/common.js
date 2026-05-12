// ── API Base URL ──────────────────────────────────────────────────────────────
export const API_BASE = 'http://localhost:8080/api/v1';

// ── DOM Helpers ───────────────────────────────────────────────────────────────
export const $ = (sel, el = document) => el.querySelector(sel);
export const $$ = (sel, el = document) => Array.from(el.querySelectorAll(sel));

// ── Query String Helper ───────────────────────────────────────────────────────
export const getQueryParam = (k) => new URLSearchParams(location.search).get(k);

// ── JSON Headers ──────────────────────────────────────────────────────────────
function jsonHeaders() {
	return { 'Content-Type': 'application/json', 'Accept': 'application/json' };
}

// ── API Fetch Wrapper ─────────────────────────────────────────────────────────
// Wraps fetch() with automatic JSON headers, response parsing, and error throwing.
export async function apiFetch(path, opts = {}) {
	const url = path.startsWith('http') ? path : `${API_BASE}${path}`;
	const init = { ...opts, headers: { ...(opts.headers || {}), ...jsonHeaders() } };
	const res = await fetch(url, init);
	const text = await res.text();
	let payload = null;
	try { payload = text ? JSON.parse(text) : null; } catch { payload = text; }
	if (!res.ok) {
		const msg = (payload && (payload.message || payload.error)) ||
			`${res.status} ${res.statusText}`;
		const err = new Error(msg);
		err.status = res.status;
		err.payload = payload;
		throw err;
	}
	return payload;
}

// ── Status Renderer ───────────────────────────────────────────────────────────
// Updates a DOM element with a styled status message ('ok', 'err', 'warn', '').
export function renderStatus(el, type, message) {
	if (!el) return;
	el.className = `status ${type}`;
	el.textContent = message;
}

// ── Clear Children ────────────────────────────────────────────────────────────
// Efficiently removes all child nodes from a container element.
export function clearChildren(el) {
	el.replaceChildren();
}

// ── Capture Movie Form ────────────────────────────────────────────────────────
// Extracts and normalizes movie data from a submitted form element.
export function captureMovieForm(form) {
	const title = form.title.value.trim();
	const year = Number(form.year.value);
	const genre = form.genre.value.trim();
	const rating = Number(form.rating.value);
	const description = form.description.value.trim();
	return { title, year, genre, rating, description };
}

// ── Capture Actor Form ────────────────────────────────────────────────────────
// Extracts and normalizes actor data from a submitted form element.
export function captureActorForm(form) {
	const firstName = form.firstName.value.trim();
	const lastName = form.lastName.value.trim();
	const rating = Number(form.rating.value);
	const bio = form.bio.value.trim();
	return { firstName, lastName, rating, bio };
}
