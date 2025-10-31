import { For, Show, createResource } from 'solid-js'
import solidLogo from './assets/solid.svg'
import viteLogo from '/vite.svg'
import './App.css'

type Post = { id: string; title: string; content: string; };

const API_URL = (import.meta as any).env?.VITE_API_URL || 'https://localhost:5001';

async function fetchPosts(): Promise<Post[]> {
  const res = await fetch(`${API_URL}/api/posts`);
  if (!res.ok) throw new Error('Failed to load posts');
  return res.json();
}

function App() {
  const [posts] = createResource(fetchPosts);

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
      <h1>Public Posts</h1>
      <Show when={!posts.loading} fallback={<p>Loading...</p>}>
        <For each={posts() || []}>{(p) => (
          <div class="card" style={{ 'text-align': 'left', margin: '12px auto', padding: '12px', width: '600px', 'max-width': '90%' }}>
            <h3>{p.title}</h3>
            <p>{p.content}</p>
          </div>
        )}</For>
      </Show>
    </>
  )
}

export default App
