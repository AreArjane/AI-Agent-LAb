import { For, Show, createResource, createSignal } from 'solid-js'
import solidLogo from './assets/solid.svg'
import viteLogo from '/vite.svg'
import './App.css'

type Post = { id: string; title: string; content: string; published?: boolean };

const API_URL = (import.meta as any).env?.VITE_API_URL || 'https://localhost:5001';

async function fetchPosts(): Promise<Post[]> {
  const res = await fetch(`${API_URL}/api/posts`);
  if (!res.ok) throw new Error('Failed to load posts');
  return res.json();
}

function App() {
  const [title, setTitle] = createSignal('');
  const [content, setContent] = createSignal('');
  const [published, setPublished] = createSignal(true);
  const [posts, { refetch }] = createResource(fetchPosts);

  async function createPost(e: Event) {
    e.preventDefault();
    await fetch(`${API_URL}/api/posts`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ title: title(), content: content(), published: published() })
    });
    setTitle('');
    setContent('');
    setPublished(true);
    await refetch();
  }

  async function deletePost(id: string) {
    await fetch(`${API_URL}/api/posts/${id}`, { method: 'DELETE' });
    await refetch();
  }

  return (
    <>
      <div>
        <a href="https://vite.dev" target="_blank">
          <img src={viteLogo} class="logo" alt="Vite logo" />
        </a>
        <a href="https://solidjs.com" target="_blank">
          <img src={solidLogo} class="logo solid" alt="Solid logo" />
        </a>
      </div>
      <h1>Admin: Manage Posts</h1>

      <form class="card" onSubmit={createPost} style={{ 'text-align': 'left', margin: '12px auto', padding: '12px', width: '600px', 'max-width': '90%' }}>
        <label>Title</label>
        <input value={title()} onInput={(e) => setTitle(e.currentTarget.value)} required />
        <label>Content</label>
        <textarea value={content()} onInput={(e) => setContent(e.currentTarget.value)} required />
        <label style={{ display: 'flex', 'align-items': 'center', gap: '8px' }}>
          <input type="checkbox" checked={published()} onChange={(e) => setPublished(e.currentTarget.checked)} />
          Published
        </label>
        <button type="submit">Create</button>
      </form>

      <Show when={!posts.loading} fallback={<p>Loading...</p>}>
        <For each={posts() || []}>{(p) => (
          <div class="card" style={{ 'text-align': 'left', margin: '12px auto', padding: '12px', width: '600px', 'max-width': '90%' }}>
            <div style={{ display: 'flex', 'justify-content': 'space-between', 'align-items': 'center' }}>
              <h3 style={{ margin: 0 }}>{p.title}</h3>
              <button onClick={() => deletePost(p.id)}>Delete</button>
            </div>
            <p>{p.content}</p>
          </div>
        )}</For>
      </Show>
    </>
  )
}

export default App
