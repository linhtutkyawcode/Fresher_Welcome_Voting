<script lang="ts">
  import logo from './assets/svelte.png'
  import Counter from './lib/Counter.svelte'
  import "./app.css";
    import { onMount } from 'svelte';

    export let forecasts = [];

    onMount(() => {
        fetch('https://localhost:5001/weatherforecast')
            .then(response => response.json())
            .then(data => {
                console.log(data);
                
                forecasts = data;
            });
    });
</script>

<main>
  <img src={logo} alt="Svelte Logo" />
  <Counter/>

<div>
    <h1>Weather forecast</h1>

    <p>This component demonstrates fetching data from the server.</p>

    {#if forecasts.length}
    <table class="table">
        <thead>
            <tr>
                <th>Date</th>
                <th>Temp. (C)</th>
                <th>Temp. (F)</th>
                <th>Summary</th>
            </tr>
        </thead>
        <tbody>
            {#each forecasts as item}
            <tr>
                <td>{ (new Date(item.date)).toLocaleDateString() }</td>
                <td>{ item.temperatureC }</td>
                <td>{ item.temperatureF }</td>
                <td>{ item.summary }</td>
            </tr>
            {/each}
        </tbody>
    </table>
    {:else}
    <p><em>Loading...</em></p>
    {/if}    
</div>
</main>
