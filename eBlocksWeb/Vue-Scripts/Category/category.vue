<template>
    <div id="category">
    <h1>Categories</h1>
 <v-data-table
    :headers="headers"
    :items="desserts"
    :items-per-page="5"
    class="elevation-1"></v-data-table>
    </div>
</template>


    <script>
        export default {
            name: "category-component",
        data(){
        return{
        headers:[
        {
         text: 'Dessert (100g serving)',
            align: 'start',
            sortable: false,
            value: 'name',
        },
            { text: 'Calories', value: 'calories' },
          { text: 'Fat (g)', value: 'fat' },
          { text: 'Carbs (g)', value: 'carbs' },
          { text: 'Protein (g)', value: 'protein' },
          { text: 'Iron (%)', value: 'iron' },
        ],
        desserts:[
        {
           name: 'Frozen Yogurt',
            calories: 159,
            fat: 6.0,
            carbs: 24,
            protein: 4.0,
            iron: '1%',
        },
        ],
        }
        },
    };
</script>