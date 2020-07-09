<template>
    <div id="category">
            <v-app id="inspire">
                <v-data-table :headers="headers"
                              :items="categories"
                              sort-by="name"
                              class="elevation-1">
                    <template v-slot:top>
                        <v-toolbar flat color="white">
                            <v-toolbar-title>Categories</v-toolbar-title>
                            <v-divider class="mx-4"
                                       inset
                                       vertical></v-divider>
                            <v-spacer></v-spacer>
                            <v-dialog v-model="dialog" max-width="500px">
                                <template v-slot:activator="{ on, attrs }">
                                    <v-btn color="primary"
                                           dark
                                           class="mb-2"
                                           v-bind="attrs"
                                           v-on="on">New Item</v-btn>
                                </template>
                                <v-card>
                                    <v-card-title>
                                        <span class="headline">{{ formTitle }}</span>
                                    </v-card-title>

                                    <v-card-text>
                                        <v-container>
                                            <v-row>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.id" label="Id"></v-text-field>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.name" label="Name"></v-text-field>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.description" label="Description"></v-text-field>
                                                </v-col>
                                                <v-col cols="12" sm="6" md="4">
                                                    <v-text-field v-model="editedItem.images" label="Image"></v-text-field>
                                                </v-col>

                                            </v-row>
                                        </v-container>
                                    </v-card-text>

                                    <v-card-actions>
                                        <v-spacer></v-spacer>
                                        <v-btn color="blue darken-1" text @click="close">Cancel</v-btn>
                                        <v-btn color="blue darken-1" text @click="save">Save</v-btn>
                                    </v-card-actions>
                                </v-card>
                            </v-dialog>
                        </v-toolbar>
                    </template>
                    <template v-slot:item.actions="{ item }">
                        <v-icon small
                                class="mr-2"
                                @click="editItem(item)">
                            mdi-pencil
                        </v-icon>
                        <v-icon small
                                @click="deleteItem(item)">
                            mdi-delete
                        </v-icon>
                    </template>
                    <template v-slot:no-data>
                        <v-btn color="primary" @click="search">Reset</v-btn>
                    </template>
                </v-data-table>
            </v-app>
        </div>

</template>


<script>
    export default {
        name: "category-component",
        data() {
            return {
                categories: [],
                dialog: false,
                headers: [
                    {
                        text: 'Id',
                        align: 'start',
                        sortable: false,
                        value: 'id',
                    },
                    { text: 'Name', value: 'name' },
                    { text: 'Description', value: 'description' },
                    { text: 'Images', value: 'images' },
                    { text: 'Actions', value: 'actions', sortable: false },
                ],
                editedIndex: -1,
                editedItem: {
                    id: '',
                    name: '',
                    description: '',
                    images: '',
                },
                defaultItem: {
                    id: '',
                    name: '',
                    description: '',
                    images: '',
                },
            }
        },
        computed: {
            formTitle() {
                return this.editedIndex === -1 ? 'New Item' : 'Edit Item'
            },
        },
        watch: {
            dialog(val) {
                val
            },
        },
        methods: {
            search: function () {
                let self = this;
                this.$axios.get('/category/search')
                    .then(function (response) {
                        self.categories = response.data.result
                    })
                    .catch(function (error) {
                        console.log(error);
                    });
            },
            editItem(item) {
                this.editedIndex = this.categories.indexOf(item)
                this.editedItem = Object.assign({}, item)
                this.dialog = true
            },

            deleteItem(item) {
                const index = this.categories.indexOf(item)
                confirm('Are you sure you want to delete this item?') && this.categories.splice(index, 1)
            },

            close() {
                this.dialog = false
                this.$nextTick(() => {
                    this.editedItem = Object.assign({}, this.defaultItem)
                    this.editedIndex = -1
                })
            },

            save() {
                if (this.editedIndex > -1) {
                    Object.assign(this.categories[this.editedIndex], this.editedItem)
                } else {
                    this.categories.push(this.editedItem)
                }
                this.close()
            }
        },
        mounted() {
            this.search()
        }
    };
</script>

