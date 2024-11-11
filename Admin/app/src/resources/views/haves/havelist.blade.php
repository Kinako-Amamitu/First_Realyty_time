@extends('layouts.app')
@section('title','所持アイテム一覧')
@section('h1','▼所持アイテム一覧▼')
@section('body')
    <table class="table table-bordered mx-auto p-2" style="width: 60%">
        @if(!empty($users))
            <tr>
                <th>ID</th>
                <th>プレイヤー名</th>
                <th>アイテム名</th>
                <th>所持数</th>
            </tr>
            @foreach($users as $user)
                @foreach ($users->items as $item)
                    <tr>
                        <td>{{$item['id']}}</td>
                        <td>{{$users->player_name}}</td>
                        <td>{{$item->item_name}}</td>
                        <td>{{$item->pivot->have}}</td>
                    </tr>
                @endforeach
            @endforeach
    </table>
    @endif
@endsection
