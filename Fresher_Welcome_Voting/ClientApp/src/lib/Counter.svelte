<script lang="ts">
  let count: number = 0
  const increment = () => {
    count += 1
  }
</script>

<button class="bg-blue-400" on:click={increment}>
  Clicks: {count}
</button>